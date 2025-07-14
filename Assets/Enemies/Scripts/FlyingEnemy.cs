using UnityEngine;

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

    private Vector3 startPos;
    private bool isDashing = false;
    private float dashTimer = 0f;
    private float cooldownTimer = 0f;
    private Vector2 dashDirection;

    void Start()
    {
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
        // Movimento flutuante + se aproximar levemente do jogador
        Vector3 hover = new Vector3(0f, Mathf.Sin(Time.time * hoverSpeed) * hoverAmplitude, 0f);
        Vector3 targetDirection = (player.position - transform.position).normalized;
        transform.position += targetDirection * moveSpeed * Time.deltaTime + hover * Time.deltaTime;
    }

    void StartDash()
    {
        isDashing = true;
        dashTimer = dashDuration;
        cooldownTimer = attackCooldown;

        dashDirection = (player.position - transform.position).normalized;
    }

    void Dash()
    {
        transform.position += (Vector3)(dashDirection * dashSpeed * Time.deltaTime);

        dashTimer -= Time.deltaTime;
        if (dashTimer <= 0f)
        {
            isDashing = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && isDashing)
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1); // Define o dano do dash
            }
        }
    }
}
