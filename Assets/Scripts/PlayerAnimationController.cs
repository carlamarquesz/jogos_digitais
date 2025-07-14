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
    private Vector2 lastDirection = Vector2.right;

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
        controls.Player.Jump.started += OnJumpStarted;
        controls.Player.Jump.canceled += OnJumpCanceled;
        controls.Player.Shoot.performed += OnShoot;
    }

    private void OnDisable()
    {
        controls.Player.Jump.started -= OnJumpStarted;
        controls.Player.Jump.canceled -= OnJumpCanceled;
        controls.Player.Shoot.performed -= OnShoot;
        controls.Disable();
    }

    private void Update()
    {
        HandleMovement();
        HandleBoostParticles();
        FlipSprite();
    }

    private void HandleMovement()
    {
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();

        if (input != Vector2.zero)
            lastDirection = input.normalized;

        bool isWalking = input.magnitude > 0.05f;
        animator.SetBool("isMoving", isWalking);
        animator.SetFloat("MoveX", input.x);
        animator.SetFloat("MoveY", input.y);

        Vector2 moveDirection = input.normalized * moveSpeed;

        if (isBoosting && input != Vector2.zero && GameManager.instance != null && GameManager.instance.TentarUsarBoost())
        {
            moveDirection += input.normalized * jumpForce;
        }

        rb.linearVelocity = moveDirection;
    }

    private void HandleBoostParticles()
    {
        var main = rocketFlame.main;
        float targetLifetime = (rb.linearVelocity.magnitude == 0 && !isBoosting) ? 1f : 2f;

        if (main.startLifetime.constant != targetLifetime)
        {
            main.startLifetime = targetLifetime;
            rocketFlame.Clear();
            rocketFlame.Play();
        }
    }

    private void FlipSprite()
    {
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();
        if (input.x > 0.1f)
            transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        else if (input.x < -0.1f)
            transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f); // Espelha para esquerda
    }

    private void OnJumpStarted(InputAction.CallbackContext ctx)
    {
        isBoosting = true;
    }

    private void OnJumpCanceled(InputAction.CallbackContext ctx)
    {
        isBoosting = false;
        if (GameManager.instance != null)
            GameManager.instance.PararBoost();
    }

    private void OnShoot(InputAction.CallbackContext ctx)
    {
        Shoot();
    }

    private void Shoot()
    {
        animator.Play("dispararPoder", 0, 0f);
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody2D projRb = projectile.GetComponent<Rigidbody2D>();
        projRb.linearVelocity = lastDirection * projectileSpeed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit!");
        }
        else if (other.CompareTag("portaSala"))
        {
            SceneManager.LoadSceneAsync("SalaJantar");
        }
        else if (other.CompareTag("portaHall"))
        {
            SceneManager.LoadSceneAsync("Game");
        }
        else if (other.CompareTag("portasala2"))
        {
            SceneManager.LoadSceneAsync("Salateste");
        }
         else if (other.CompareTag("Puzzle1"))
        {
            SceneManager.LoadSceneAsync("PUZZLE1");
        }
        // else if (other.CompareTag("Energia"))
        // {
        //     ColetarEnergia();
        //     Destroy(other.gameObject);
        // }
    }

    // private void ColetarEnergia()
    // {
    //     energyCollected++;
    //     if (energyCollected >= energyPiecesToRecharge)
    //     {
    //         energyCollected = 0;
    //         if (GameManager.instance != null)
    //             GameManager.instance.RecarregarBoost();
    //     }
    // }
}
