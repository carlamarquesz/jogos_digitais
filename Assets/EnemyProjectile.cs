using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth ph = other.GetComponent<PlayerHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
                Debug.Log("Colidiu com: " + other.name);

            }

            Destroy(gameObject); // Destroi o projétil
        }
        
    }
}
