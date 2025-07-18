using UnityEngine;

public class FireMagic : MonoBehaviour
{
    public float speed = 5f;
    public int damage = 10;
    public Transform player; // arraste o transform do player no inspector

    void Update()
    {
        if (player != null)
        {
            Vector3 dir = (player.position - transform.position).normalized;
            transform.position += dir * speed * Time.deltaTime;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Aplica dano (se o player tiver um script de vida)
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }

            // Destroi a magia
            Destroy(gameObject);
        }
        else //if (other.CompareTag("Obstacle")) // opcional: se bater numa parede
        {
            Destroy(gameObject);
        }
    }
}
