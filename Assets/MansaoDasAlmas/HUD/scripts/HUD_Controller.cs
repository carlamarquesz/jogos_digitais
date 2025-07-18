using UnityEngine;
using UnityEngine.UI;

public class HUD_Controller : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    void Start()
    {
        Screen.SetResolution(1280, 720, false); // false = janela, true = tela cheia

        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();
        if (playerHealth != null)
        {
            SetHealth(playerHealth.currentHealth);
        }
        else
        {
            Debug.LogWarning("PlayerHealth instance not found.");
        }
    }

    void Update()
    {
    }
}
