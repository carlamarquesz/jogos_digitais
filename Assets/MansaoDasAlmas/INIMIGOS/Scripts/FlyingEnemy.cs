using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class FlyingEnemy : MonoBehaviour
{
    public Transform player;
    public float hoverSpeed = 2f;
    public float hoverAmplitude = 0.5f;
    public float moveSpeed = 2f;

    public float attackRange = 5f;
    public float dashSpeed = 10f;
    public float dashDuration = 0.5f;
    public float attackCooldown = 2f;

    private Rigidbody2D rb;
    private Vector3 startPos;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;
    private Vector2 dashDirection;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true; // Garante que ele não gire
        startPos = transform.position;
    }

    void Update()
    {
        if (player == null) return;

        cooldownTimer -= Time.deltaTime;

        if (!isDashing)
        {
            HoverMovement();

            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            if (distanceToPlayer <= attackRange && cooldownTimer <= 0f)
            {
                StartDash();
            }
        }
        else
        {
            Dash();
        }
    }

    void HoverMovement()
    {
        // Movimento flutuante + aproximação do jogador
        Vector3 hover = new Vector3(0f, Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude, 0f);
        Vector2 direction = (player.position - transform.position).normalized;
        Vector2 velocity = (direction * moveSpeed) + (Vector2)(hover);
        rb.linearVelocity = velocity;
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        cooldownTimer = attackCooldown;

        dashDirection = (player.position - transform.position).normalized;
        rb.linearVelocity = dashDirection * dashSpeed;
    }

    void Dash()
    {
        rb.linearVelocity = dashDirection * dashSpeed;

        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0f)
        {
            isDashing = false;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing && collision.collider.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }
    }
}
