using UnityEngine;
using TMPro;

public class PiperGameManager : MonoBehaviour
{
    public GameObject pipeHolder;
    public GameObject[] Pipes;

    public TextMeshProUGUI progressText;

    [SerializeField]
    public int totalPipes = 0;

    private int correctedPipes = 0;

    void Start()
    {
        totalPipes = pipeHolder.transform.childCount;
        Pipes = new GameObject[totalPipes];

        correctedPipes = 0; // Zera antes de contar

        for (int i = 0; i < totalPipes; i++)
        {
            Pipes[i] = pipeHolder.transform.GetChild(i).gameObject;

            pipeScript pipeScriptComponent = Pipes[i].GetComponent<pipeScript>();
            if (pipeScriptComponent != null && pipeScriptComponent.IsPlaced())
            {
                correctedPipes++;
            }
        }

        UpdateProgressText();
    }

    public void correctMove()
    {
        correctedPipes++;
        correctedPipes = Mathf.Clamp(correctedPipes, 0, totalPipes);

        Debug.Log("Correct Move | Total corrigidos: " + correctedPipes);
        UpdateProgressText();

        if (correctedPipes == totalPipes)
        {
            Debug.Log("✅ Todos os encaixes foram colocados corretamente!");
            // Lógica de vitória aqui
        }
    }

    public void incorrectMove()
    {
        correctedPipes--;
        correctedPipes = Mathf.Clamp(correctedPipes, 0, totalPipes);

        Debug.Log("Incorrect Move | Total corrigidos: " + correctedPipes);
        UpdateProgressText();
    }

    void UpdateProgressText()
    {
        if (progressText != null)
        {
            progressText.text = $"Total de peças: {totalPipes}\npeças corretos: {correctedPipes}";
        }
    }
}
