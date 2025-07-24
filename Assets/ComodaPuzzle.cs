using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ComodaPuzzle : MonoBehaviour
{
    public static ComodaPuzzle instance { get; private set; }

    [Header("UI")]
    public TMP_InputField inputField;
    public GameObject canvasCarta;
    public GameObject canvasDialogo;
    public TMP_Text texto;
    public Button botaoConfirmar;

    [Header("Configuração")]
    public string ordemCorreta = "BDAC";

    private bool _comodaDestravada = false;
    public bool comodaDestravada => _comodaDestravada; // Getter público seguro

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("Mais de um ComodaPuzzle na cena. Um será destruído.");
            Destroy(this);
            return;
        }
        instance = this;
    }

    public void VerificarSequencia()
    {
        string resposta = inputField.text.ToUpper().Trim();

        if (string.IsNullOrEmpty(resposta))
            return;

        if (resposta == ordemCorreta)
        {
            _comodaDestravada = true;
            canvasCarta.SetActive(false);

            if (ContadorManager.instance != null)
                ContadorManager.instance.IncrementarItens();
            else
                Debug.LogWarning("ContadorManager.instance está nulo!");

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

    private System.Collections.IEnumerator FecharMensagemAposTempo(float tempo)
    {
        yield return new WaitForSecondsRealtime(tempo);
        canvasDialogo.SetActive(false);
        Time.timeScale = 1f;
    }
}
