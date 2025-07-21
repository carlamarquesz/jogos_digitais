using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DialogoManager : MonoBehaviour
{
    public GameObject canvasDialogo;  
    public Button botaoContinuar;

    private static bool dialogoMostrado = false; 

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
}
