using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Collections;

public class PlayerHealth : MonoBehaviour
{
    public HUD_Controller hud;

    [HideInInspector] public int maxHealth = 100;
    [HideInInspector] public int currentHealth = 100;

    [HideInInspector] public int maxMana = 100;
    [HideInInspector] public int currentMana = 100;

    [Header("Referências")]
    public GameObject gameOverUI;
    public HealthBarController healthBar;
    public TextMeshProUGUI textoGameOverTempo;
    public TextMeshProUGUI textoGameOverPontos;

    public static int saveHealth = -1;

    // Configuração da regeneração de mana
    public int manaRegenAmount = 5;
    public float manaRegenInterval = 1f;

    void Awake()
    {
        if (hud == null)
        {
            hud = FindFirstObjectByType<HUD_Controller>();
        }

        if (hud == null)
        {
            Debug.LogError("HUD_Controller não encontrado!");
            return;
        }

        if (saveHealth < 0)
        {
            currentHealth = maxHealth;
            saveHealth = maxHealth;
        }
        else
        {
            currentHealth = saveHealth;
        }

        hud.SetMaxHealth(maxHealth);
        hud.SetHealth(currentHealth);

        hud.SetMaxMana(maxMana);
        hud.SetMana(currentMana);
    }

    void Start()
    {
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        // Inicia a coroutine de regeneração
        StartCoroutine(RegenerarMana());
    }

    IEnumerator RegenerarMana()
    {
        while (true)
        {
            yield return new WaitForSeconds(manaRegenInterval);
            RegenerarManaTick();
        }
    }

    void RegenerarManaTick()
    {
        if (currentMana < maxMana)
        {
            currentMana += manaRegenAmount;
            currentMana = Mathf.Clamp(currentMana, 0, maxMana);
            hud.SetMana(currentMana);
            // Debug opcional:
            // Debug.Log($"Mana regenerada: {manaRegenAmount}, Mana atual: {currentMana}");
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

    public void UseMana(int amount)
    {
        if (currentMana < amount)
        {
            Debug.LogWarning("Tentativa de usar mais mana do que a disponível!");
            return;
        }

        currentMana -= amount;
        currentMana = Mathf.Clamp(currentMana, 0, maxMana);
        hud.SetMana(currentMana);
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
