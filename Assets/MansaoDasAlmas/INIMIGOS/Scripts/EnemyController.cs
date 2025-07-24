using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public Transform center;
    public Transform player;

    public float moveSpeed = 1f;
    public float shootIntervalMin = 1f;
    public float shootIntervalMax = 3f;
    public float projectileSpeed = 5f;
    private float direction = -1f;

    private float shootTimer;
    private EnemySlow enemySlow;

    private Rigidbody2D rb;

    // Valores base para poder restaurar após o buff
    private float baseMoveSpeed;
    private float baseProjectileSpeed;

    void Start()
    {
        enemySlow = GetComponent<EnemySlow>();
        shootTimer = Random.Range(shootIntervalMin, shootIntervalMax);
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;

        baseMoveSpeed = moveSpeed;
        baseProjectileSpeed = projectileSpeed;
    }

    void Update()
    {
        // Verifica se está no horário noturno e aplica o buff
        if (GameClock.Instance != null && GameClock.Instance.IsNightTime)
        {
            moveSpeed = baseMoveSpeed * 2f;
            projectileSpeed = baseProjectileSpeed * 2f;
        }
        else
        {
            moveSpeed = baseMoveSpeed;
            projectileSpeed = baseProjectileSpeed;
        }

        if (player != null)
        {
            Vector2 dir = (player.position - transform.position).normalized;

            float velocidadeAtual = (enemySlow != null) ? enemySlow.GetVelocidadeAtual() : moveSpeed;

            rb.linearVelocity = dir * velocidadeAtual;  // Corrigido para velocity

            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            center.rotation = Quaternion.Euler(0f, 0f, angle);
        }
        else
        {
            rb.linearVelocity = Vector2.zero;
        }

        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = Random.Range(shootIntervalMin, shootIntervalMax);
        }
    }

    void Shoot()
    {
        if (projectilePrefab != null && shootPoint != null)
        {
            GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody2D rbProj = projectile.GetComponent<Rigidbody2D>();
            if (rbProj != null)
            {
                rbProj.linearVelocity = shootPoint.right * projectileSpeed;  // Corrigido para velocity
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            Debug.Log("Colidiu com o jogador!");

            PlayerHealth playerHealth = collision.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                int dano = 1; // dano ao jogador na colisão
                playerHealth.TakeDamage(dano);
            }
        }
    }

    public void SetDirection(float dir)
    {
        direction = dir;
        if (dir < 0)
            transform.localScale = new Vector3(-1, 1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }
}
