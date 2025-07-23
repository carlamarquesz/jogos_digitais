using UnityEngine;
using System.Collections;  // <-- importante para IEnumerator

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public GameObject explosionPrefab;
    private bool isSlowed = false;
    private float originalSpeed = 1f;
    private float slowedSpeed = 0.4f;

    private PlayerMovement playerMovement;
    private bool morreu = false;  // FLAG para evitar múltiplas mortes

    void Start()
    {
        currentHealth = maxHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            playerMovement = playerObj.GetComponent<PlayerMovement>();
        }
    }

    public void TakeDamage(MagicType magicType)
    {
        if (morreu) return;  // Ignora dano após morte

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
        Debug.Log("Inimigo desacelerado!");
        yield return new WaitForSeconds(2f);
        Debug.Log("Inimigo voltou à velocidade normal.");
        isSlowed = false;
    }

    void Die()
    {
        if (morreu) return; // Evita executar múltiplas vezes
        morreu = true;

        Debug.Log("Inimigo morreu!");

        if (playerMovement != null)
        {
            playerMovement.MonstroDerrotado();
        }

        EnemyDeath deathScript = GetComponent<EnemyDeath>();
        if (deathScript != null)
        {
            deathScript.Die();
        }
        else
        {
            Destroy(gameObject);
        }

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
