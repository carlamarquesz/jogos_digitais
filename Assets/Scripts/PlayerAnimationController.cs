using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 2f;

    public MagiaScript magiaScript; // Referência no inspector para o controlador de magias

    private Animator animator;
    private Rigidbody2D rb;
    private PlayerControls controls;
    public ParticleSystem rocketFlame;
    private bool avisoMostrado = false; // <-- REFERENCIA PARA A CENA DO CHEFÃO

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

    public Image encontrouItemImage;       // A imagem que dá fade
    public CanvasGroup encontrouCanvasGroup; // Requer componente CanvasGroup no objeto
    public GameObject canvasDialogo;
    public TMP_Text dialogoTexto;
    public Button botaoFecharDialogo;

    public ComodaPuzzle puzzle; // arraste no Inspector

    // Propriedade para pegar a magia atual (prefab) do MagiaScript
    private GameObject CurrentMagiaPrefab
    {
        get
        {
            if (magiaScript != null && magiaScript.magias.Length > 0)
            {
                return magiaScript.magias[magiaScript.currentMagiaIndex];
            }
            return null;
        }
    }

    private void Awake()
    {
        controls = new PlayerControls();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
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
        {
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // Direita (normal)
        }
        else if (input.x < -0.1f)
        {
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f); // Esquerda (espelhado)
        }
    }

    private void Shoot()
{
    animator.Play("dispararPoder", 0, 0f);

    GameObject prefab = CurrentMagiaPrefab;
    if (prefab == null)
    {
        Debug.LogWarning("Nenhuma magia selecionada para disparar.");
        return;
    }

    GameObject proj = Instantiate(prefab, shootPoint.position, shootPoint.rotation);

    Magia magia = proj.GetComponent<Magia>();
    if (magia != null)
    {
        // Aqui ajustamos a direção conforme o lado do personagem
        float direction = transform.localScale.x > 0 ? 1f : -1f;
        magia.direcao = new Vector2(direction, 0f); // Direita ou esquerda
        magia.velocidade = projectileSpeed;

        // Se quiser girar a magia visualmente também:
        Vector3 escala = proj.transform.localScale;
        escala.x = Mathf.Abs(escala.x) * direction;
        proj.transform.localScale = escala;
    }
    else
    {
        Debug.LogWarning("Prefab da magia não possui o componente Magia.");
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
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit!");
        }
        if (other.CompareTag("portaSala"))
        {
            SceneManager.LoadScene("SalaJantar");
        }
        if (other.CompareTag("portaHall"))
        {
            SceneManager.LoadScene("Game");
        }
        if (other.CompareTag("portal-principal"))
        {
            MostrarAvisoPortao();
        }
        if (other.CompareTag("carta"))
        {
            MostrarCarta();
        }
        if (other.CompareTag("comoda"))
        {
            if (ComodaPuzzle.comodaDestravada)
            {
                StartCoroutine(RevelarItemComFade());
            }
            else
            {
                ComodaPuzzle.instance.MostrarMensagem("A cômoda está trancada. Resolva o enigma primeiro.");
                Debug.Log("A cômoda está trancada. Resolva o enigma primeiro.");
            }
        }
    }
}
