using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;  // Importante para UI

public class WinScript1 : MonoBehaviour
{
    public int totalPieces = 5;
    private int currentPoints = 0;

    [Header("Nomes das cenas")]
    public string cenaAnterior = "Menu";
    public string proximaCena = "CenaFinal";

    public Text pressFText;  // Arraste o objeto Text aqui no Inspector

    void Start()
    {
        // Esconde o texto no início
        if (pressFText != null)
            pressFText.gameObject.SetActive(false);
    }

    void Update()
    {
        // Mostra o texto "Pressione F" quando o jogador estiver "pronto" para voltar
        // (Aqui só um exemplo simples: texto sempre aparece)
        if (pressFText != null)
            pressFText.gameObject.SetActive(true);

        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene("Corredor1");
        }
    }

    public void AddPoint()
    {
        currentPoints++;
        Debug.Log($"Peça encaixada! Total: {currentPoints}/{totalPieces}");

        if (currentPoints >= totalPieces)
        {
            WinGame();
        }
    }

    void WinGame()
    {
        Debug.Log("Você venceu o jogo! Parabéns!");
        SceneManager.LoadScene("Corredor");
    }
}
