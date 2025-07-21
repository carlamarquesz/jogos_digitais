using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PiperGameManager2 : MonoBehaviour
{
    public GameObject pipeHolder;
    public GameObject[] Pipes;

    public TextMeshProUGUI progressText;

    [SerializeField]
    public int totalPipes = 0;

    private int correctedPipes = 0;

    [Header("Nomes das cenas")]
    public string cenaProxima = "Game";     // Nome da cena para onde ir ao vencer
    public string cenaAnterior = "Corredor4"; // Cena para voltar ao pressionar F

    private bool venceu = false;

    void Start()
    {
        totalPipes = pipeHolder.transform.childCount;
        Pipes = new GameObject[totalPipes];
        correctedPipes = 0;

        for (int i = 0; i < totalPipes; i++)
        {
            Pipes[i] = pipeHolder.transform.GetChild(i).gameObject;

            // Conta apenas as peças que já estão corretas
            var pipeScriptComponent = Pipes[i].GetComponent<pipeScript2>();
            if (pipeScriptComponent != null && pipeScriptComponent.IsPlaced())
            {
                correctedPipes++;
            }
        }

        UpdateProgressText();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            SceneManager.LoadScene(cenaAnterior);
        }

        // Verifica vitória apenas aqui
        if (!venceu && correctedPipes == totalPipes)
        {
            venceu = true;
            ContadorManager.instance.IncrementarItens();
            Debug.Log("✅ Todos os encaixes foram colocados corretamente!");
            SceneManager.LoadScene(cenaProxima);
            
        }
    }

    public void correctMove()
    {
        correctedPipes = Mathf.Clamp(++correctedPipes, 0, totalPipes);
        Debug.Log("Correct Move | Total corrigidos: " + correctedPipes);
        UpdateProgressText();
    }

    public void incorrectMove()
    {
        correctedPipes = Mathf.Clamp(--correctedPipes, 0, totalPipes);
        Debug.Log("Incorrect Move | Total corrigidos: " + correctedPipes);
        UpdateProgressText();
    }

    void UpdateProgressText()
    {
        if (progressText != null)
        {
            progressText.text = $"Total de peças: {totalPipes}\nPeças corretas: {correctedPipes}";
        }
    }
}
