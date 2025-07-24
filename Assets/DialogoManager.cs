using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogoManager : MonoBehaviour
{
    public static DialogoManager instance;  // Instância acessível de fora

    public GameObject canvasDialogo;
    public Button botaoContinuar;

    private static bool dialogoMostrado = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (!dialogoMostrado && SceneManager.GetActiveScene().name == "Game")
        {
            StartCoroutine(MostrarDialogo());
            dialogoMostrado = true;
        }
    }

    IEnumerator MostrarDialogo()
    {
        yield return new WaitForSeconds(2f);
        canvasDialogo.SetActive(true);
        Time.timeScale = 0f;

        botaoContinuar.onClick.AddListener(FecharDialogo);
    }

    void FecharDialogo()
    {
        canvasDialogo.SetActive(false);
        Time.timeScale = 1f;
    }

    // ✅ Este método resolve o erro CS1061
    public bool FoiConcluido(string id)
    {
        return dialogoMostrado;
    }
}
