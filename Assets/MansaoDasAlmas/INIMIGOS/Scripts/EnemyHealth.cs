using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public GameObject explosionPrefab;
    private bool isSlowed = false;
    private float originalSpeed = 1f; // define ou vincule à velocidade real do inimigo
    private float slowedSpeed = 0.4f;

    void Start()
    {
        currentHealth = maxHealth;
    }

   public void TakeDamage(MagicType magicType)
{
    int damage = 1;

    switch (magicType)
    {
        case MagicType.Fire:
            damage = 3;
            break;
        case MagicType.Ice:
            damage = 2;
            if (!isSlowed) StartCoroutine(ApplySlowEffect());
            break;
        case MagicType.Darkness:
            damage = 1;
            break;
    }

    currentHealth -= damage;
    currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

    if (currentHealth <= 0)
        Die();
}


    IEnumerator ApplySlowEffect()
    {
        isSlowed = true;

        // Exemplo: se você tiver um script de movimento, diminua a velocidade lá.
        // Ex: GetComponent<EnemyMovement>().speed = slowedSpeed;
        Debug.Log("Inimigo desacelerado!");

        yield return new WaitForSeconds(2f);

        // Restaurar velocidade
        // Ex: GetComponent<EnemyMovement>().speed = originalSpeed;
        Debug.Log("Inimigo voltou à velocidade normal.");

        isSlowed = false;
    }

    void Die()
    {
        Debug.Log("Inimigo morreu!");
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
