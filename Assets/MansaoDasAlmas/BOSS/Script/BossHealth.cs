using UnityEngine;
using System.Collections;


public class BossHealth : MonoBehaviour
{
    public int maxHealth = 10;
    public int currentHealth;

    private bool isSlowed = false;
    private float originalSpeed = 1f;
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

        Debug.Log($"Boss recebeu magia {magicType} causando {damage} de dano.");
        currentHealth -= damage;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    IEnumerator ApplySlowEffect()
    {
        isSlowed = true;
        Debug.Log("Boss desacelerado!");

        // Aqui você pode modificar velocidade do boss se houver script de movimento
        // Exemplo: GetComponent<BossMovement>().speed = slowedSpeed;

        yield return new WaitForSeconds(2f);

        // Restaurar velocidade
        // Exemplo: GetComponent<BossMovement>().speed = originalSpeed;

        Debug.Log("Boss voltou à velocidade normal.");
        isSlowed = false;
    }

    void Die()
    {
        Debug.Log("Boss morreu!");
        Destroy(gameObject);
    }
}
