using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 2f;

    public MagiaScript magiaScript;

    private float originalSpeed;
    private int slowHits = 0;
    private float slowTimer = 0f;
    private float slowDuration = 2f;

    private Animator animator;
    private Rigidbody2D rb;
    private PlayerControls controls;
    public ParticleSystem rocketFlame;
    private bool avisoMostrado = false;

    public Transform shootPoint;
    public Transform enemy;
    private bool isBoosting = false;

    public float projectileSpeed = 5f;
    public int energyPiecesToRecharge = 3;
    private int energyCollected = 0;

    public GameObject canvasAvisoPortao;
    public Button botaoFecharAviso;

    public GameObject canvasCarta;
    public Button botaoFecharCarta;

    public Image encontrouItemImage;
    public CanvasGroup encontrouCanvasGroup;
    public MonstroInicial monstroInicialScript;
    private int monstrosDerrotados = 0;
    public int monstrosParaDialogo = 5;
    public GameObject canvasDialogo;
    public TMP_Text dialogoTexto;
    public Button botaoFecharDialogo;

    public ComodaPuzzle puzzle;

    private Dictionary<string, string> portasComRestricao = new Dictionary<string, string>()
    {
        { "portaSala", "SalaJantar" },
        { "EscadaDireita", "Corredor1" },
        { "EscadaEsquerda", "Corredor3" },
        { "Salao", "Game" },
        { "portaHall", "Game" },
        { "Porta1C", "PortaCorredor1" },
        { "Porta2C", "PortaCorredor2" },
        { "Porta3C", "PUZZLE2Stones" },
        { "Porta1C1", "PortaCorredor1C1" },
        { "Porta2C1", "PUZZLE1Stones" },
        { "Porta3C1", "PortaCorredor2C1" },
        { "Porta1C3", "PortaCorredor2 3" },
        { "Porta2C3", "PUZZLE2Circuitos" },
        { "Porta3C3", "PortaCorredor1 3" },
        { "Porta1C4", "PUZZLE1Circuitos" },
        { "PortalCorredor1", "Corredor" },
        { "PortalCorredor1C1", "Corredor1" },
        { "PortalCorredor3C1", "Corredor3" }
    };

    private void Awake()
    {
        controls = new PlayerControls();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        originalSpeed = moveSpeed;
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Player.Jump.started += ctx => { isBoosting = true; };
        controls.Player.Jump.canceled += ctx =>
        {
            isBoosting = false;
            GameManager.instance.PararBoost();
        };

        controls.Player.Shoot.performed += ctx => Shoot();
    }

    private void OnDisable()
    {
        controls.Player.Jump.started -= ctx => isBoosting = true;
        controls.Player.Jump.canceled -= ctx => isBoosting = false;
        controls.Player.Shoot.performed -= ctx => Shoot();
        controls.Disable();
    }

    private void Update()
    {
        if (slowTimer > 0)
        {
            slowTimer -= Time.deltaTime;
            if (slowTimer <= 0f)
            {
                moveSpeed = originalSpeed;
                slowHits = 0;
                slowTimer = 0;
            }
        }

        Vector2 input = controls.Player.Move.ReadValue<Vector2>();
        bool isWalking = input.magnitude > 0.05f;
        animator.SetBool("isMoving", isWalking);
        animator.SetFloat("MoveX", input.x);
        animator.SetFloat("MoveY", input.y);
        Vector2 moveDirection = input.normalized * moveSpeed;

        if (isBoosting && input != Vector2.zero && GameManager.instance.TentarUsarBoost())
        {
            moveDirection += input.normalized * jumpForce;
        }

        rb.linearVelocity = moveDirection;

        var main = rocketFlame.main;
        float targetLifetime = (input == Vector2.zero && !isBoosting) ? 1f : 2f;
        if (main.startLifetime.constant != targetLifetime)
        {
            main.startLifetime = targetLifetime;
            rocketFlame.Clear();
            rocketFlame.Play();
        }

        if (input.x > 0.1f)
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        else if (input.x < -0.1f)
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
    }

    private void Shoot()
    {
        if (magiaScript == null) return;

        GameObject prefab = magiaScript.magias[magiaScript.currentMagiaIndex];
        if (prefab == null) return;

        int custo = (magiaScript.custoManaPorMagia != null && magiaScript.currentMagiaIndex < magiaScript.custoManaPorMagia.Length)
                    ? magiaScript.custoManaPorMagia[magiaScript.currentMagiaIndex]
                    : 10;

        if (magiaScript.playerHealth.currentMana < custo) return;

        magiaScript.playerHealth.UseMana(custo);
        animator.Play("dispararPoder", 0, 0f);
        GameObject proj = Instantiate(prefab, shootPoint.position, shootPoint.rotation);

        Magia magia = proj.GetComponent<Magia>();
        if (magia != null)
        {
            float direction = transform.localScale.x > 0 ? 1f : -1f;
            magia.direcao = new Vector2(direction, 0f);
            magia.velocidade = projectileSpeed;
            Vector3 escala = proj.transform.localScale;
            escala.x = Mathf.Abs(escala.x) * direction;
            proj.transform.localScale = escala;
        }
    }

    public void ModifySpeedCumulative(float slowPercent, float duration)
    {
        slowHits++;
        float effectiveMultiplier = 1f - Mathf.Min(slowHits, 2) * slowPercent;
        moveSpeed = originalSpeed * effectiveMultiplier;
        slowTimer = duration;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.tag;

        if (portasComRestricao.ContainsKey(tag))
        {
            if (MonstroInicial.dialogoFinalizado)
            {
                SceneManager.LoadScene(portasComRestricao[tag]);
            }
            else
            {
                MostrarAvisoDialogoNaoFinalizado();
            }
            return;
        }

        if (tag == "portal-principal")
        {
            if (ContadorManager.instance.PodeEntrarNoPortal())
                SceneManager.LoadScene("Ritual");
            else
                MostrarAvisoPortao();
        }

        if (tag == "carta")
        {
            if (DialogosManager.instance != null && !DialogosManager.instance.FoiConcluido("dialogo_sala_jantar"))
                MostrarCarta();
        }

        if (tag == "comoda")
        {
            if (ComodaPuzzle.instance == null)
            {
                Debug.LogWarning("ComodaPuzzle.instance é null!");
                return;
            }

            if (ComodaPuzzle.instance.comodaDestravada)
            {
                if (DialogoManager.instance != null && DialogoManager.instance.FoiConcluido("dialogo_sala_jantar"))
                {
                    ComodaPuzzle.instance.MostrarMensagem("Você já capturou o item.");
                }
                else
                {
                    StartCoroutine(RevelarItemComFade());
                    if (DialogosManager.instance != null)
                    {
                        DialogosManager.instance.MarcarDialogoComoConcluido("dialogo_sala_jantar");
                    }
                }
            }
            else
            {
                ComodaPuzzle.instance.MostrarMensagem("A cômoda está trancada. Resolva o enigma primeiro.");
            }
        }

        if (tag == "Enemy")
        {
            Debug.Log("Enemy hit!");
        }
    }

    private void MostrarAvisoPortao()
    {
        Time.timeScale = 0f;
        canvasAvisoPortao.SetActive(true);
        botaoFecharAviso.onClick.RemoveAllListeners();
        botaoFecharAviso.onClick.AddListener(FecharAvisoPortao);
    }

    private void FecharAvisoPortao()
    {
        canvasAvisoPortao.SetActive(false);
        Time.timeScale = 1f;
    }

    private void MostrarCarta()
    {
        Time.timeScale = 0f;
        canvasCarta.SetActive(true);
        botaoFecharCarta.onClick.RemoveAllListeners();
        botaoFecharCarta.onClick.AddListener(FecharCarta);
    }

    private void FecharCarta()
    {
        canvasCarta.SetActive(false);
        Time.timeScale = 1f;
    }

    private void MostrarAvisoDialogoNaoFinalizado()
    {
        Time.timeScale = 0f;
        canvasDialogo.SetActive(true);
        dialogoTexto.text = "Uma força sombria bloqueia seu caminho...";

        botaoFecharDialogo.onClick.RemoveAllListeners();
        botaoFecharDialogo.onClick.AddListener(() =>
        {
            canvasDialogo.SetActive(false);
            Time.timeScale = 1f;
        });
    }

    IEnumerator RevelarItemComFade()
    {
        Time.timeScale = 0f;

        encontrouItemImage.gameObject.SetActive(true);
        encontrouCanvasGroup.alpha = 0f;

        float t = 0f;
        while (t < 1f)
        {
            t += Time.unscaledDeltaTime;
            encontrouCanvasGroup.alpha = Mathf.Lerp(0f, 1f, t);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(5f);

        encontrouItemImage.gameObject.SetActive(false);

        canvasDialogo.SetActive(true);
        dialogoTexto.text = "Você encontrou um item de força: O anel mágico.";

        botaoFecharDialogo.onClick.RemoveAllListeners();
        botaoFecharDialogo.onClick.AddListener(() =>
        {
            canvasDialogo.SetActive(false);
            Time.timeScale = 1f;
        });
    }

    public void MonstroDerrotado()
    {
        monstrosDerrotados++;
        Debug.Log($"Monstro derrotado! Total: {monstrosDerrotados}");

        if (monstrosDerrotados >= monstrosParaDialogo)
        {
            if (monstroInicialScript != null)
            {
                monstroInicialScript.IniciarDialogo();
            }
            else
            {
                Debug.LogWarning("MonstroInicial não está atribuído no PlayerMovement.");
            }
        }
    }

    private void MostrarDialogoPosMonstros()
    {
        if (canvasDialogo.activeSelf) return;

        Time.timeScale = 0f;
        canvasDialogo.SetActive(true);
        dialogoTexto.text = "Você derrotou monstros suficientes! O caminho está liberado.";

        botaoFecharDialogo.onClick.RemoveAllListeners();
        botaoFecharDialogo.onClick.AddListener(() =>
        {
            canvasDialogo.SetActive(false);
            Time.timeScale = 1f;
        });
    }
}
