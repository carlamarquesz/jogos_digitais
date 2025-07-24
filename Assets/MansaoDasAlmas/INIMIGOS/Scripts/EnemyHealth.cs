using UnityEngine;
using System.Collections;  // Para IEnumerator

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public GameObject explosionPrefab;

    private bool isSlowed = false;
    private bool morreu = false;           // Evita múltiplas mortes
    private bool estaInvencivel = false;   // Invencibilidade temporária para evitar múltiplos danos seguidos
    public float duracaoInvencibilidade = 0.3f;  // Tempo que o inimigo fica invencível após levar dano

    private PlayerMovement playerMovement;

    void Start()
    {
        currentHealth = maxHealth;

        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
            playerMovement = playerObj.GetComponent<PlayerMovement>();
    }

    public void TakeDamage(MagicType magicType)
    {
        if (morreu || estaInvencivel)
            return;

        int damage = 1;

        switch (magicType)
        {
            case MagicType.Fire:
                damage = 3;
                break;
            case MagicType.Ice:
                damage = 2;
                if (!isSlowed)
                    StartCoroutine(ApplySlowEffect());
                break;
            case MagicType.Darkness:
                damage = 1;
                break;
        }

        Debug.Log($"[{gameObject.name}] Recebido {damage} de dano ({magicType}). Vida antes: {currentHealth}/{maxHealth}");

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        StartCoroutine(TornarInvencivelTemporariamente());
        StartCoroutine(FlashDamage());

        Debug.Log($"[{gameObject.name}] Vida restante: {currentHealth}/{maxHealth}");

        if (currentHealth <= 0)
            Die();
    }

    private IEnumerator TornarInvencivelTemporariamente()
    {
        estaInvencivel = true;
        yield return new WaitForSeconds(duracaoInvencibilidade);
        estaInvencivel = false;
    }

    private IEnumerator ApplySlowEffect()
    {
        isSlowed = true;
        Debug.Log("Inimigo desacelerado!");
        yield return new WaitForSeconds(2f);
        Debug.Log("Inimigo voltou à velocidade normal.");
        isSlowed = false;
    }

    private IEnumerator FlashDamage()
    {
        var sr = GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            Color originalColor = sr.color;
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = originalColor;
        }
    }

    private void Die()
    {
        if (morreu)
            return;

        morreu = true;
        Debug.Log($"[{gameObject.name}] Inimigo morreu!");

        if (playerMovement != null)
            playerMovement.MonstroDerrotado();

        EnemyDeath deathScript = GetComponent<EnemyDeath>();
        if (deathScript != null)
            deathScript.Die();
        else
            Destroy(gameObject);

        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
    }
}
