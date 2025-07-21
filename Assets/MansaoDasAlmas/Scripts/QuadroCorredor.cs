using UnityEngine;
using TMPro;
using System.Collections;

public class QuadroInterativo : MonoBehaviour
{
    [Header("Referências UI")]
    public GameObject imagemZoomUI;
    public GameObject painelLegenda;
    public TextMeshProUGUI legendaUI;
    public TextMeshProUGUI avisoFecharUI;

    [Header("Configuração")]
    [TextArea]
    public string textoLegenda = "Um enigma antigo: Fogo, Gelo, Praga, Água e Vento.";

    private bool playerPerto = false;
    private bool quadroAberto = false;
    private Coroutine mostrarLegendaCoroutine;

    [Header("Configuração do Texto")]
    public float tempoEntreLetras = 0.05f;

    private GameObject player;
    private PlayerMovement movimentoJogador;

    void Start()
    {
        FecharQuadro();
    }

    void Update()
    {
        if (playerPerto && Input.GetKeyDown(KeyCode.E))
        {
            if (quadroAberto)
                FecharQuadro();
            else
                AbrirQuadro();
        }
    }

    private void AbrirQuadro()
    {
        if (imagemZoomUI != null)
            imagemZoomUI.SetActive(true);

        if (painelLegenda != null)
        {
            painelLegenda.SetActive(true);
            painelLegenda.transform.SetAsLastSibling();
        }

        if (avisoFecharUI != null)
            avisoFecharUI.text = "Pressione [E] para fechar";

        quadroAberto = true;

        if (movimentoJogador != null)
            movimentoJogador.enabled = false; // Impede movimento

        if (mostrarLegendaCoroutine != null)
            StopCoroutine(mostrarLegendaCoroutine);

        mostrarLegendaCoroutine = StartCoroutine(MostrarLegenda(textoLegenda));
    }

    private void FecharQuadro()
    {
        if (imagemZoomUI != null)
            imagemZoomUI.SetActive(false);

        if (painelLegenda != null)
            painelLegenda.SetActive(false);

        if (legendaUI != null)
            legendaUI.text = "";

        if (avisoFecharUI != null)
            avisoFecharUI.text = "";

        quadroAberto = false;

        if (movimentoJogador != null)
            movimentoJogador.enabled = true; // Reativa movimento

        if (mostrarLegendaCoroutine != null)
        {
            StopCoroutine(mostrarLegendaCoroutine);
            mostrarLegendaCoroutine = null;
        }
    }

    private IEnumerator MostrarLegenda(string texto)
    {
        legendaUI.text = "";

        foreach (char letra in texto)
        {
            legendaUI.text += letra;
            yield return new WaitForSeconds(tempoEntreLetras);
        }

        mostrarLegendaCoroutine = null;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPerto = true;
            player = collision.gameObject;
            movimentoJogador = player.GetComponent<PlayerMovement>();

            if (avisoFecharUI != null)
                avisoFecharUI.text = "Pressione [E] para abrir";
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerPerto = false;
            FecharQuadro();
            player = null;
            movimentoJogador = null;
        }
    }
}
