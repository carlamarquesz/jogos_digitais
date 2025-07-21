using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement; 

public class DialogoBoss : MonoBehaviour
{
    public GameObject prefabMonstro;
    public Transform spawnPosition;
    public GameObject canvasMonstro;
    public TextMeshProUGUI textoDialogo;
    public Button botaoContinuar;
    public GameObject imagemPoder;

    private GameObject monstroInstanciado;
    private int falaAtual = 0;

    private static bool monstroJaApareceu = false;

    public TextMeshProUGUI nomePersonagem;


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
            new Fala { nome = "Desconhecido", texto = "Você chegou... mas já é tarde demais." },
            new Fala { nome = "Whisper", texto = "O que você fez com ela?!" },
            new Fala { nome = "Desconhecido", texto = "Ela me convidou. Abriu as portas da alma... e eu entrei." },
            new Fala { nome = "Whisper", texto = "Ela é só uma criança! Você a envenenou com sua escuridão!" },
            new Fala { nome = "Desconhecido", texto = "Eu apenas mostrei o que já estava dentro dela..." },
            new Fala { nome = "Menina", texto = "Alguém...? Está escuro aqui... estou com medo..." },
            new Fala { nome = "Whisper", texto = "Lute! Não deixe ele controlar quem você é!" },
            new Fala { nome = "Desconhecido", texto = "Quer libertá-la? Derrote-me, e talvez ela volte." },
            new Fala { nome = "Whisper", texto = "Eu juro... vou arrancar essa sombra de dentro dela, nem que eu queime junto." }
        };
        if (!monstroJaApareceu)
        {
            StartCoroutine(ApagarMonstro());
            monstroJaApareceu = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    IEnumerator ApagarMonstro()
    {
        yield return new WaitForSeconds(1f); 

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
            Debug.Log($"Exibindo fala: {falas[falaAtual].nome}: {falas[falaAtual].texto}");

            switch (falas[falaAtual].nome)
            {
                case "Desconhecido":
                    nomePersonagem.text = $"<color=#FF0000>{falas[falaAtual].nome}</color>";
                    break;
                case "Garota":
                    nomePersonagem.text = $"<color=#ddb892>{falas[falaAtual].nome}</color>";
                    break;
                default:
                    nomePersonagem.text = falas[falaAtual].nome;
                    break;
            }

            textoDialogo.text = falas[falaAtual].texto;
        }
    }


    void ProximaFala()
    {
        falaAtual++;

        if (falaAtual == falas.Count - 1)
        {
            StartCoroutine(MostrarImagemPoder());
        }
        else if (falaAtual < falas.Count)
        {
            MostrarFala();
        }
        else
        {
            StartCoroutine(FecharCanvasETrocarCena());
        }
    }

    IEnumerator MostrarImagemPoder()
    {
        canvasMonstro.SetActive(false);
        imagemPoder.SetActive(true);

        CanvasGroup cg = imagemPoder.GetComponent<CanvasGroup>();
        if (cg == null)
        {
            cg = imagemPoder.AddComponent<CanvasGroup>();
        }

        cg.alpha = 0f;
        float duracao = 1f;
        float tempo = 0f;

        while (tempo < duracao)
        {
            tempo += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(0f, 1f, tempo / duracao);
            yield return null;
        }

        cg.alpha = 1f;
        yield return new WaitForSecondsRealtime(3f);

        imagemPoder.SetActive(false);
        canvasMonstro.SetActive(true);
        MostrarFala();
    }

    IEnumerator FecharCanvasETrocarCena()
    {
        canvasMonstro.SetActive(false);
        Time.timeScale = 1f;

        if (monstroInstanciado != null)
        {
            Destroy(monstroInstanciado);
        }
        DialogosManager.instance.MarcarDialogoComoConcluido("dialogo_boss_ritual");


        yield return new WaitForSeconds(1f); 

        SceneManager.LoadScene("SalaBoss"); 
    }
}
