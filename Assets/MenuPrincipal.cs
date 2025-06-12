using UnityEngine;
using TMPro;

public class MenuPrincipal : MonoBehaviour
{
    public GameObject painelRanking;
    public TextMeshProUGUI textoRanking;

    public GameObject painelInstrucoes;


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
        foreach (string linha in lista)
        {
            textoRanking.text += linha + "\n";
        }
    }


    public void FecharRanking()
    {
        painelRanking.SetActive(false);
    }

    public void MostrarInstrucoes()
    {
        painelInstrucoes.SetActive(true); 
    }


    public void FecharInstrucoes()
    {
        painelInstrucoes.SetActive(false);
    }

    public void ZerarRanking()
    {
        RankingManager.ZerarRanking();
        MostrarRanking(); // Atualiza visual
    }
}
