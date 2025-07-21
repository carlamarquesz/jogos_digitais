using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ComodaPuzzle : MonoBehaviour
{
    [Header("UI")]
    public TMP_InputField inputField;
    public GameObject canvasCarta;

    public GameObject canvasDialogo;   
    public TMP_Text texto;            
    public Button botaoConfirmar;      

    [Header("Configuração")]
    public string ordemCorreta = "BDAC";
    public static bool comodaDestravada = false;
    public static ComodaPuzzle instance;

    private void Awake()
    {
        instance = this;
    }

    public void VerificarSequencia()
    {
        string resposta = inputField.text.ToUpper().Trim();
        if (string.IsNullOrEmpty(resposta))
        {
            return; 
        }
        if (resposta == ordemCorreta)
        {
            ComodaPuzzle.comodaDestravada = true;
            canvasCarta.SetActive(false);
            ContadorManager.instance.IncrementarItens();
            MostrarMensagem("A cômoda foi destravada com sucesso!");
        }
        else
        {
            MostrarMensagem("Nada aconteceu. A sequência parece incorreta.");
        }
        inputField.text = "";

        canvasCarta.SetActive(false);

    }

    public void MostrarMensagem(string mensagem)
    {
        texto.text = mensagem;
        canvasDialogo.SetActive(true);
        Time.timeScale = 0f;

        StartCoroutine(FecharMensagemAposTempo(2f));
    }

    System.Collections.IEnumerator FecharMensagemAposTempo(float tempo)
    {
        yield return new WaitForSecondsRealtime(tempo); 

        canvasDialogo.SetActive(false);
        Time.timeScale = 1f;
    }

}
