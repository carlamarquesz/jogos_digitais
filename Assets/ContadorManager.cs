using UnityEngine;

public class ContadorManager : MonoBehaviour
{
    private static ContadorManager _instance;

    public static ContadorManager instance
    {
        get
        {
            if (_instance == null)
            {
                // Tenta encontrar na cena
                _instance = FindObjectOfType<ContadorManager>();

                if (_instance == null)
                {
                    // Cria novo GameObject com o script
                    GameObject singletonObj = new GameObject("ContadorManager");
                    _instance = singletonObj.AddComponent<ContadorManager>();
                    Debug.Log("ContadorManager criado automaticamente.");
                }

                DontDestroyOnLoad(_instance.gameObject);
            }
            return _instance;
        }
    }

    public int itensEncontrados = 0;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
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
