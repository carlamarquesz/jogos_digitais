using UnityEngine;
using TMPro; // TextMeshPro

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Pontuação")]
    public int pontos = 0;
    public TextMeshProUGUI textoPontuacao;

    [Header("Tempo")]
    public float tempo = 0f;
    public bool jogoAtivo = true;
    public TextMeshProUGUI textoTempo;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (!jogoAtivo) return;

        tempo += Time.deltaTime;

        if (textoTempo != null)
        {
            int minutos = Mathf.FloorToInt(tempo / 60f);
            int segundos = Mathf.FloorToInt(tempo % 60f);
            textoTempo.text = $"Tempo: {minutos:D2}:{segundos:D2}";
        }
    }

    public void AddPoint()
    {
        pontos++;

        if (textoPontuacao != null)
        {
            textoPontuacao.text = pontos.ToString("D3");
        }
    }

    public void FimDeJogo()
    {
        jogoAtivo = false;
        RankingManager.SalvarPontuacao(pontos, tempo);

        Debug.Log($"Jogo Finalizado | Tempo: {tempo:F2} | Pontos: {pontos}");

    }
}
