using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DialogoManager : MonoBehaviour
{
    public GameObject canvasDialogo;  
    public Button botaoContinuar;    

    private void Start()
    {
        StartCoroutine(MostrarDialogo());
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
