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


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.SetResolution(1280, 720, false); // false = janela, true = tela cheia

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            SetHealth(playerHealth.currentHealth);
        }
        else
        {
            Debug.LogWarning("PlayerHealth instance not found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
