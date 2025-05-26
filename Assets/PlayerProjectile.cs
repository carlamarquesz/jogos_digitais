using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth ph = other.GetComponent<EnemyHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
                Debug.Log("Colidiu com: " + other.name);

            }

            Destroy(gameObject); // Destroi o projétil
        }

    }
}
