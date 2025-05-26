using UnityEngine;

public class HealthBarController : MonoBehaviour
{
    public Sprite[] lifeSprites; // lista das imagens do sprite da barra de vida
    public SpriteRenderer barRenderer;

    public void UpdateBar(int currentHealth)
    {
        int index = Mathf.Clamp(currentHealth, 0, lifeSprites.Length - 1);
        barRenderer.sprite = lifeSprites[index];
    }
}
