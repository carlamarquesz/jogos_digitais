using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    public Slider barraDeVida;

    public void SetMaxHealth(int max)
    {
        if (barraDeVida != null)
        {
            barraDeVida.maxValue = max;
            barraDeVida.value = max;
        }
    }

    public void SetHealth(int health)
    {
        if (barraDeVida != null)
        {
            barraDeVida.value = health;
        }
    }
}
