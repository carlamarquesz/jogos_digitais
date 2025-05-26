using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    // Animaçao da nave sendo destruida
    public float lifetime = 1f; 

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
