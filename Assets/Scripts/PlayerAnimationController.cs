using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 2f;

    private Animator animator;
    private Rigidbody2D rb;
    private PlayerControls controls;
    public ParticleSystem rocketFlame;

    public GameObject projectilePrefab;
    public Transform shootPoint;
    public Transform enemy;
    private bool isBoosting = false;

    public float projectileSpeed = 5f;
    public int energyPiecesToRecharge = 3;
    private int energyCollected = 0;

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

        // Verifica se está se movendo
        bool isWalking = input.magnitude > 0.05f;
        animator.SetBool("isMoving", isWalking);

        // Define valores dos parâmetros de direção
        animator.SetFloat("MoveX", input.x);
        animator.SetFloat("MoveY", input.y);

        // Define velocidade do movimento
        Vector2 moveDirection = input.normalized * moveSpeed;

        // Aplica boost se disponível
        if (isBoosting && input != Vector2.zero && GameManager.instance.TentarUsarBoost())
        {
            moveDirection += input.normalized * jumpForce;
        }

        rb.linearVelocity = moveDirection; // Corrigido para `rb.velocity` (mais comum que `linearVelocity`)

        // Controla partículas do boost
        var main = rocketFlame.main;
        float targetLifetime = (input == Vector2.zero && !isBoosting) ? 1f : 2f;
        if (main.startLifetime.constant != targetLifetime)
        {
            main.startLifetime = targetLifetime;
            rocketFlame.Clear();
            rocketFlame.Play();
        }

        // Inverte sprite baseado na direção horizontal
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
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        //rb.linearVelocity = Vector2.up * projectileSpeed;
        float direction = Mathf.Sign(transform.localScale.x);
        rb.linearVelocity = new Vector2(direction * projectileSpeed, 0f); // move na horizontal

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
    }
}
