using UnityEngine;
using UnityEngine.UI;

public class HUD_Controller : MonoBehaviour
{
    public Slider sliderLife;
    public Slider sliderMana;

    public void SetMaxHealth(int health)
    {
        sliderLife.maxValue = health;
    }

    public void SetHealth(int health)
    {
        sliderLife.value = health;
    }
    public void SetMaxMana(int mana)
    {
        sliderMana.maxValue = mana;
    }

    public void SetMana(int mana)
    {
        sliderMana.value = mana;
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Screen.SetResolution(1280, 720, false); // false = janela, true = tela cheia

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            SetHealth(playerHealth.currentHealth);
            SetMana(playerHealth.currentMana);
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
