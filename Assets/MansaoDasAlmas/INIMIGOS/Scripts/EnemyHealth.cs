using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public GameObject explosionPrefab;

    void Start()
    {
        currentHealth = maxHealth; 
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Ataque de " + amount);

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Inimigo levou dano! Vida: " + currentHealth); 

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Inimigo morreu!");
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
