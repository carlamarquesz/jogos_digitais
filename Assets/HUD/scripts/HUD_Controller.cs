using UnityEngine;
using UnityEngine.UI;

public class HUD_Controller : MonoBehaviour
{
    public Slider sliderLife;
    public Slider sliderMana;

    public void SetMaxHealth(int health)
    {
        sliderLife.maxValue = health;
        sliderLife.value = health;
    }

    public void SetHealth(int health)
    {
        sliderLife.value = health;
    }

    public void SetMaxMana(int mana)
    {
        sliderMana.maxValue = mana;
        sliderMana.value = mana;
    }

    public void SetMana(int mana)
    {
        sliderMana.value = mana;
    }

    void Start()
    {
        Screen.SetResolution(1280, 720, false);

        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth != null)
        {
            SetMaxHealth(playerHealth.maxHealth);
            SetHealth(playerHealth.currentHealth);
            SetMaxMana(playerHealth.maxMana);
            SetMana(playerHealth.currentMana);
        }
        else
        {
            Debug.LogWarning("PlayerHealth instance not found.");
        }
    }
}
