using UnityEngine;
using UnityEngine.UI;

public class BossHealthBarUI : MonoBehaviour
{
    public Image fillImage;  // Referência para o Image do Fill
    private int maxHealth;

    public void SetMaxHealth(int max)
    {
        maxHealth = max;
        SetHealth(max);
    }

    public void SetHealth(int currentHealth)
    {
        if (fillImage != null && maxHealth > 0)
        {
            fillImage.fillAmount = (float)currentHealth / maxHealth;
        }
    }
}
