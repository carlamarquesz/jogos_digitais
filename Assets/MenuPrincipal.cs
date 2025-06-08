using UnityEngine;
using TMPro;

public class MenuPrincipal : MonoBehaviour
{
    public GameObject painelRanking;
    public TextMeshProUGUI textoRanking;

    public void IniciarJogo()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("1Parte_Atividade");
    }

    public void SairDoJogo()
    {
        Application.Quit();
    }

    public void MostrarRanking()
    {
        painelRanking.SetActive(true);

        string[] lista = RankingManager.ObterRankingFormatado();
        textoRanking.text = "== TOP 10 ==\n\n";
        foreach (string linha in lista)
        {
            textoRanking.text += linha + "\n";
        }
    }

    public void FecharRanking()
    {
        painelRanking.SetActive(false);
    }

    public void ZerarRanking()
    {
        RankingManager.ZerarRanking();
        MostrarRanking(); // Atualiza visual
    }
}
