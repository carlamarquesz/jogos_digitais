using UnityEngine;

public class PlayerMana : MonoBehaviour
{
    public float maxMana = 100f;
    public float currentMana;

    private void Start()
    {
        currentMana = maxMana;
    }

    public bool UseMana(float amount)
    {
        if (currentMana >= amount)
        {
            currentMana -= amount;
            return true;
        }
        return false;
    }

    public void DrainMana(float amount)
{
    currentMana = Mathf.Max(currentMana - amount, 0f);
}


    public void RestoreMana(float amount)
    {
        currentMana = Mathf.Min(currentMana + amount, maxMana);
    }
}
