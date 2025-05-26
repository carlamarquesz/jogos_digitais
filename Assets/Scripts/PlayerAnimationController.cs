using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 2f;
    public int maxJumps = 2;

    private int jumpCount;
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerControls controls;
    public ParticleSystem rocketFlame;

    public GameObject projectilePrefab;     // Prefab do projétil
    public Transform shootPoint;            // Ponto de onde o projétil vai sair
    public Transform enemy;                // Referência ao inimigo

    public float projectileSpeed = 5f;

    private void Awake()
    {
        controls = new PlayerControls();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Jump.performed += ctx => Jump();
        controls.Player.Shoot.performed += ctx => Shoot(); // <-- Adicionado aqui
    }

    private void OnDisable()
    {
        controls.Player.Jump.performed -= ctx => Jump();
        controls.Player.Shoot.performed -= ctx => Shoot(); // <-- E removido aqui
        controls.Disable();
    }

    private void Update()
    {
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();

        // Movimento 2D: horizontal (x) e vertical (y)
        Vector3 moveDirection = new Vector3(input.x, input.y, 0);
        transform.Translate(moveDirection * moveSpeed * Time.deltaTime);

        // Controle do Particle System
        var main = rocketFlame.main;
        if (input == Vector2.zero)
        {
            if (main.startLifetime.constant != 1f)
            {
                main.startLifetime = 1f;
                rocketFlame.Clear();
                rocketFlame.Play();
            }
        }
        else
        {
            if (main.startLifetime.constant != 2f)
            {
                main.startLifetime = 2f;
                rocketFlame.Clear();
                rocketFlame.Play();
            }
        }

        // Flip apenas se houver movimento horizontal
        if (input.x > 0)
            transform.localScale = new Vector3(0.5f, 0.5f, 1);
        else if (input.x < 0)
            transform.localScale = new Vector3(-0.5f, 0.5f, 1);

        // Resetar pulo se tocar no chão
        if (Mathf.Abs(rb.linearVelocity.y) < 0.01f)
            jumpCount = 0;
    }

    private void Jump()
    {
        if (jumpCount < maxJumps)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;
            animator.SetBool("Jump", true);
        }
    }

    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        rb.linearVelocity = Vector2.up * projectileSpeed;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit!");
            // Aqui você pode chamar o método TakeDamage do jogador, se quiser
        }
    }
}
