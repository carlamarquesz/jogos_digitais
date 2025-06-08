using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 3;
    public int currentHealth;

    [Header("Referências")]
    public GameObject gameOverUI;
    public HealthBarController healthBar;
    public TextMeshProUGUI textoGameOverTempo;
    public TextMeshProUGUI textoGameOverPontos;

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
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        if (healthBar != null)
        {
            healthBar.UpdateBar(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Time.timeScale = 0f;

        // Finaliza o jogo no GameManager
        if (GameManager.instance != null)
        {
            GameManager.instance.FimDeJogo();

            // Atualiza texto do tempo
            if (textoGameOverTempo != null)
            {
                float tempoFinal = GameManager.instance.tempo;
                int min = Mathf.FloorToInt(tempoFinal / 60f);
                int seg = Mathf.FloorToInt(tempoFinal % 60f);
                textoGameOverTempo.text = $"Tempo: {min:D2}:{seg:D2}";
            }

            // Atualiza texto de pontos
            if (textoGameOverPontos != null)
            {
                textoGameOverPontos.text = $"Pontos: {GameManager.instance.pontos:D3}";
            }
        }

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }
    }

    public void RestartScene()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
