using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        // Verifica se atingiu inimigo comum ou o Boss
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            // Primeiro tenta pegar EnemyHealth
            EnemyHealth ph = other.GetComponent<EnemyHealth>();
            if (ph != null)
            {
                ph.TakeDamage(damage);
                Debug.Log("Colidiu com inimigo: " + other.name);
            }

            // Se for Boss e tiver outro script, também pode fazer isso:
            BossHealth bh = other.GetComponent<BossHealth>();
            if (bh != null)
            {
                bh.TakeDamage(damage);
                Debug.Log("Colidiu com Boss: " + other.name);
            }

            Destroy(gameObject);
        }
    }
}
