using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;
    public GameObject gameOverUI;

    public HealthBarController healthBar;

    void Start()
    {
        currentHealth = maxHealth;

        if (healthBar != null)
        {
            healthBar.UpdateBar(currentHealth);
        }
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
    }

    public void TakeDamage(int amount)
    {
        Debug.Log("Ataque de " + amount);

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log("Jogador levou dano! Vida: " + currentHealth);

        if (healthBar != null)
        {
            healthBar.UpdateBar(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    //void Die()
    //{
    //    Debug.Log("Jogador morreu! Reiniciando cena...");
    //    Invoke(nameof(RestartScene), 0.5f); // Espera 0.5 segundo antes de reiniciar
    //}

    void Die()
    {
        Debug.Log("Jogador morreu! Mostrando tela de Game Over...");

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        Time.timeScale = 0f; // pausa o jogo
    }

    public void RestartScene()
    {
        Time.timeScale = 1f; // retoma o tempo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

}
