using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public GameObject projectilePrefab;     // Prefab do projétil
    public Transform shootPoint;            // Ponto de onde o projétil vai sair
    public Transform player;               

    public float moveSpeed = 1f;
    public float shootIntervalMin = 1f;
    public float shootIntervalMax = 3f;
    public float projectileSpeed = 5f;

    private float shootTimer;

    void Start()
    {
        // Define o tempo inicial para o próximo tiro
        shootTimer = Random.Range(shootIntervalMin, shootIntervalMax);
    }

    void Update()
    {
        if (player != null)
        {
            // Rotaciona o inimigo para olhar para o jogador
            Vector2 direction = (player.position - transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle); 
            transform.position += (Vector3)(direction * moveSpeed * Time.deltaTime);
        }

        // Atirar em intervalos
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = Random.Range(shootIntervalMin, shootIntervalMax);
        }
    }

    void Shoot()
    {
        // Cria o projétil com a mesma rotação do ponto de tiro
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        rb.linearVelocity = shootPoint.right * projectileSpeed;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Colidiu com o jogador!");

            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(playerHealth.currentHealth); // Tira toda a vida
            }
        }
    }
}
