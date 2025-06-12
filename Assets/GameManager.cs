using UnityEngine;
using TMPro;

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

    [Header("Energia")]
    public int energiaColetada = 0;
    public int boostCargas = 0;
    public int energiaPorCarga = 3;
    public TextMeshProUGUI textoEnergia; // Referência opcional à UI de boost

    [Header("Tempo de Boost")]
    public float tempoMaximoPorCarga = 8f;
    private float tempoBoostAtual = 0f;
    private bool boostAtivo = false;


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
        if (boostAtivo)
        {
            tempoBoostAtual += Time.deltaTime;

            if (tempoBoostAtual >= tempoMaximoPorCarga)
            {
                boostCargas = 0;
                energiaColetada = 0;
                tempoBoostAtual = 0f;
                boostAtivo = false;
                Debug.Log("Boost encerrado. Sistema reiniciado.");
            }

            AtualizarTextoEnergia(); // <- isso mantém a UI em tempo real
        }




    }

    public void AddPoint()
    {
        pontos++;
        if (textoPontuacao != null)
            textoPontuacao.text = pontos.ToString("D3");
    }

    public void FimDeJogo()
    {
        jogoAtivo = false;
        RankingManager.SalvarPontuacao(pontos, tempo);
        Debug.Log($"Jogo Finalizado | Tempo: {tempo:F2} | Pontos: {pontos}");
    }

    public void ColetarEnergia()
    {
        // Se já tem boost e energia cheia, não deve coletar
        if (boostCargas >= 1 && energiaColetada >= energiaPorCarga)
        {
            Debug.Log("Coleta bloqueada: boost ativo e energia cheia.");
            return;
        }

        // Só coleta se ainda estiver abaixo do limite
        if (energiaColetada < energiaPorCarga)
        {
            energiaColetada++;
        }

        // Se atingiu o máximo, carrega boost (mas mantém 3/3 visível até o boost ser usado)
        if (energiaColetada == energiaPorCarga && boostCargas == 0)
        {
            boostCargas = 1;
        }

        AtualizarTextoEnergia(); // Chama depois de todo o processo
    }






    public bool TentarUsarBoost()
    {
        if (boostCargas > 0)
        {
            boostAtivo = true;
            return true;
        }

        return false;
    }
    public void PararBoost()
    {
        boostAtivo = false;
    }
    void AtualizarTextoEnergia()
    {
        if (textoEnergia == null) return;

        string boostTexto;

        if (boostAtivo)
        {
            float restante = Mathf.Max(tempoMaximoPorCarga - tempoBoostAtual, 0f);
            boostTexto = $"Boost: {restante:F1}s";
        }
        else if (energiaColetada == energiaPorCarga && boostCargas > 0)
        {
            // Boost pronto, mas ainda não usado
            boostTexto = $"Boost: {tempoMaximoPorCarga:F1}s";
        }
        else
        {
            boostTexto = "Boost: ---";
        }

        textoEnergia.text = $"{boostTexto} | Energia: {energiaColetada}/{energiaPorCarga}";
    }




}
