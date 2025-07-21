using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GarotaDialogo : MonoBehaviour
{
    public GameObject prefabMonstro;
    public Transform spawnPosition;
    public GameObject canvasMonstro;
    public TextMeshProUGUI textoDialogo;
    public Button botaoContinuar;
    public GameObject imagemPoder;
    public GameObject imagemPoder2;  
    public TextMeshProUGUI nomePersonagem;

    private GameObject monstroInstanciado;
    private int falaAtual = 0;
    private static bool monstroJaApareceu = false;

    [System.Serializable]
    public class Fala
    {
        public string nome;
        public string texto;
    }

    public List<Fala> falas = new List<Fala>();

    void Start()
    {
        falas = new List<Fala>
        {
            new Fala { nome = "Whisper", texto = "Você está segura agora. Qual seu nome...?" },
            new Fala { nome = "Menina", texto = "Evryn... Me chamo Evryn." },
            new Fala { nome = "Whisper", texto = "Evryn, seu pai me enviou... ele nunca perdeu a esperança de te encontrar. O que aconteceu com você?" },

            new Fala { nome = "Evryn", texto = "Eu vim até esta mansão buscando respostas... uma voz me prometeu alívio." },
            new Fala { nome = "Evryn", texto = "Ela dizia que podia apagar a dor da perda de minha mãe, do vazio que me consumia por dentro." },
            new Fala { nome = "Evryn", texto = "Mas era mentira. Ela se alimentou do que restava em mim... até que eu não soubesse mais quem eu era." },

            new Fala { nome = "Evryn", texto = "A cada noite, ouvia passos. Pessoas tentando me alcançar. Salvadores." },
            new Fala { nome = "Evryn", texto = "A mansão os devorava um por um. E cada alma que tombava... era usada contra o próximo." },

            new Fala { nome = "Whisper",     texto = "Você não é culpada. A escuridão engana até os mais puros... Mas você resistiu. E agora, os monstros que atacavam neste lugar encontram enfim a liberdade." },
            new Fala { nome = "Whisper", texto = "Venha, Evryn. Vamos deixar esse lugar para trás." },
            new Fala { nome = "Evryn", texto = "Sim. Mas os que vieram antes... eu nunca esquecerei. Eles foram a ponte até a luz." },

            new Fala { nome = "Narrador", texto = "E assim, Evryn e Whisper sairam da mansão onde tantas almas se apagaram." }

        };

        if (!monstroJaApareceu)
        {
            StartCoroutine(IniciarCena());
            monstroJaApareceu = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator IniciarCena()
    {
        yield return MostrarImagemPoder();

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(ApagarMonstro());
    }

    IEnumerator MostrarImagemPoder()
    {
        CanvasGroup cg1 = imagemPoder.GetComponent<CanvasGroup>();
        if (cg1 == null)
            cg1 = imagemPoder.AddComponent<CanvasGroup>();

        CanvasGroup cg2 = imagemPoder2.GetComponent<CanvasGroup>();
        if (cg2 == null)
            cg2 = imagemPoder2.AddComponent<CanvasGroup>();

        imagemPoder.SetActive(true);
        imagemPoder2.SetActive(true); 

        cg1.alpha = 0f;
        cg2.alpha = 0f;

        float duracao = 1f;
        float tempo = 0f;
         
        while (tempo < duracao)
        {
            tempo += Time.unscaledDeltaTime;
            cg1.alpha = Mathf.Lerp(0f, 1f, tempo / duracao);
            yield return null;
        }

        yield return new WaitForSecondsRealtime(2f);  
         
        tempo = 0f;
        while (tempo < duracao)
        {
            tempo += Time.unscaledDeltaTime;
            float t = tempo / duracao;
            cg1.alpha = Mathf.Lerp(1f, 0f, t); 
            cg2.alpha = Mathf.Lerp(0f, 1f, t); 
            yield return null;
        }

        cg1.alpha = 0f;
        cg2.alpha = 1f;
        imagemPoder.SetActive(false);  

        yield return new WaitForSecondsRealtime(2f);  
         
        tempo = 0f;
        while (tempo < duracao)
        {
            tempo += Time.unscaledDeltaTime;
            cg2.alpha = Mathf.Lerp(1f, 0f, tempo / duracao);
            yield return null;
        }

        imagemPoder2.SetActive(false);
    }



    IEnumerator ApagarMonstro()
    {
        monstroInstanciado = Instantiate(prefabMonstro, spawnPosition.position, Quaternion.identity);
        yield return new WaitForSeconds(2f);

        canvasMonstro.SetActive(true);
        Time.timeScale = 0f;

        MostrarFala();
        botaoContinuar.onClick.AddListener(ProximaFala);
    }

    void MostrarFala()
    {
        if (falaAtual < falas.Count)
        {
            var fala = falas[falaAtual];

            switch (fala.nome)
            {
                case "Menina":
                case "Evryn":
                    nomePersonagem.text = $"<color=#ddb892>{fala.nome}</color>";
                    break;
                default:
                    nomePersonagem.text = fala.nome;
                    break;
            }

            textoDialogo.text = fala.texto;
        }
    }

    void ProximaFala()
    {
        falaAtual++;

        if (falaAtual < falas.Count)
        {
            MostrarFala();
        }
        else
        {
            StartCoroutine(FinalizarDialogo());
        }
    }

    IEnumerator FinalizarDialogo()
    {
        canvasMonstro.SetActive(false);
        Time.timeScale = 1f;

        if (monstroInstanciado != null)
            Destroy(monstroInstanciado);

        yield return null;
        Debug.Log("Diálogo finalizado.");
        if (DialogosManager.instance != null)
        {
            DialogosManager.instance.MarcarDialogoComoConcluido("dialogo_evryn");
        }
        SceneManager.LoadScene("MenuPrincipal"); 

    }
}
