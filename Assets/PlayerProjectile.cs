using UnityEngine;

public class PlayerProjectile : MonoBehaviour
{
    public MagicType magicType = MagicType.Fire;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            EnemyHealth enemyHealth = other.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(magicType);
                Debug.Log("Colidiu com inimigo: " + other.name);
            }

            BossHealth bossHealth = other.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(magicType);
                Debug.Log("Colidiu com Boss: " + other.name);
            }

            Destroy(gameObject);
        }
    }
}
