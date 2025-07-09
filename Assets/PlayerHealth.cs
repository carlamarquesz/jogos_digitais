using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerHealth : MonoBehaviour
{   
    public HUD_Controller hud;
    [HideInInspector]
    public int maxHealth;
    [HideInInspector]
    public int currentHealth;

    [Header("Refer�ncias")]
    public GameObject gameOverUI;
    public HealthBarController healthBar;
    public TextMeshProUGUI textoGameOverTempo;
    public TextMeshProUGUI textoGameOverPontos;

    public static int saveHealth = -1;

    void Awake()
    {
        if (saveHealth < 0)
        {
            hud.SetMaxHealth(maxHealth);
            PlayerHealth.saveHealth = maxHealth;
        }
        else
        {
            currentHealth = PlayerHealth.saveHealth;
            hud.SetHealth(currentHealth);
        }
    }
    void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }
    }

    public void TakeDamage(int amount)
    {
        currentHealth -= amount;
        saveHealth = currentHealth;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        hud.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        Time.timeScale = 0f; 
        if (GameManager.instance != null)
        {
            GameManager.instance.FimDeJogo(); 
            if (textoGameOverTempo != null)
            {
                float tempoFinal = GameManager.instance.tempo;
                int min = Mathf.FloorToInt(tempoFinal / 60f);
                int seg = Mathf.FloorToInt(tempoFinal % 60f);
                textoGameOverTempo.text = $"Tempo: {min:D2}:{seg:D2}";
            } 
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
