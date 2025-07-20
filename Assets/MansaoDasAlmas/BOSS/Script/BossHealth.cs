using UnityEngine;
using System.Collections;

public class BossHealth : MonoBehaviour
{
    public int maxHealth = 20;
    public int currentHealth;
    public GameObject explosionPrefab;

    private bool isSlowed = false;
    private float originalSpeed = 1f;
    private float slowedSpeed = 0.4f;

    // Referência à UI da barra de vida do boss
    public BossHealthBarUI barraVidaUI;

    void Start()
    {
        currentHealth = maxHealth;

        // Inicializa a barra de vida
        if (barraVidaUI != null)
        {
            barraVidaUI.SetMaxHealth(maxHealth);
            barraVidaUI.SetHealth(currentHealth);
        }
    }

    public void TakeDamage(MagicType magicType)
    {
        int damage = 1;

        switch (magicType)
        {
            case MagicType.Fire:
                damage = 5;
                break;
            case MagicType.Ice:
                damage = 3;
                if (!isSlowed) StartCoroutine(ApplySlowEffect());
                break;
            case MagicType.Darkness:
                damage = 2;
                break;
            default:
                damage = 1;
                break;
        }

        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        // Atualiza barra de vida na UI
        if (barraVidaUI != null)
        {
            barraVidaUI.SetHealth(currentHealth);
        }

        Debug.Log($"Boss recebeu {damage} de dano por magia {magicType}. Vida atual: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }

    IEnumerator ApplySlowEffect()
    {
        isSlowed = true;

        // Exemplo: GetComponent<BossMovement>().speed = slowedSpeed;
        Debug.Log("Boss desacelerado!");

        yield return new WaitForSeconds(2f);

        // Exemplo: GetComponent<BossMovement>().speed = originalSpeed;
        Debug.Log("Boss voltou à velocidade normal.");
        isSlowed = false;
    }

    void Die()
    {
        Debug.Log("Boss morreu!");
        if (explosionPrefab != null)
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
