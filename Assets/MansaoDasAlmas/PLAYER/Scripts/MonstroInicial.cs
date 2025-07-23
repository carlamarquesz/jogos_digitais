// Atualizado: MonstroInicial.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MonstroInicial : MonoBehaviour
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
    public static bool dialogoFinalizado = false;

    private List<string> falas = new List<string>
    {
        "Seu poder... você não é nada sem ele, mago patético.",
        "Espalhei suas relíquias por esta mansão amaldiçoada. Cada cômodo guarda um fragmento do que você era.",
        "Se quiser entrar no salão principal e encontrar a garota... terá que procurar seus itens por cada canto escuro deste lugar."
    };

    void Start() { }

    public void IniciarDialogo()
    {
        if (!monstroJaApareceu)
        {
            monstroJaApareceu = true;
            StartCoroutine(ApagarMonstro());
        }
    }

    IEnumerator ApagarMonstro()
    {
        yield return new WaitForSeconds(2f);

        monstroInstanciado = Instantiate(prefabMonstro, spawnPosition.position, Quaternion.identity);

        yield return new WaitForSeconds(2f);

        canvasMonstro.SetActive(true);
        Time.timeScale = 0f;

        MostrarFala();
        botaoContinuar.onClick.RemoveAllListeners();
        botaoContinuar.onClick.AddListener(ProximaFala);
    }

    void MostrarFala()
    {
        if (falaAtual < falas.Count)
        {
            textoDialogo.text = falas[falaAtual];
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
            FecharCanvas();
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

    void FecharCanvas()
    {
        canvasMonstro.SetActive(false);
        Time.timeScale = 1f;

        if (monstroInstanciado != null)
        {
            Destroy(monstroInstanciado);
        }

        dialogoFinalizado = true;
    }
}
