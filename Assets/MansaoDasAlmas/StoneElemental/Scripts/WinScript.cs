using UnityEngine;
using UnityEngine.SceneManagement;

public class WinScript : MonoBehaviour
{
    public int totalPieces = 5;  // Total de peças corretas a encaixar
    private int currentPoints = 0;

    [Header("Nomes das cenas")]
    public string cenaAnterior = "Corredor";      // Cena para onde voltar com tecla F
    public string proximaCena = "CenaFinal";  // Cena que será carregada após vencer

    void Update()
    {
        // Tecla F para voltar
        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene("Corredor");
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
        SceneManager.LoadScene("Game");
    }
}
