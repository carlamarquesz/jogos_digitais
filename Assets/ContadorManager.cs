using UnityEngine;

public class ContadorManager : MonoBehaviour
{
    public static ContadorManager instance;

    public int itensEncontrados = 0;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject); 
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); 
    }

    public bool PodeEntrarNoPortal()
    {
        return itensEncontrados >= 5;
    }

    public void IncrementarItens()
    {
        itensEncontrados++;
        Debug.Log("Itens encontrados: " + itensEncontrados);
    }
}
