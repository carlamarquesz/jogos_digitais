using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CutsceneController : MonoBehaviour
{
    [System.Serializable]
    public class CutsceneStep
    {
        public GameObject cutsceneObject;
        [HideInInspector] public CanvasGroup canvasGroup;
    }

    [Header("Configurações")]
    public CutsceneStep[] cutsceneSteps;
    public float fadeDuration = 1f;
    public string nextSceneName = "Game";

    private int currentIndex = 0;

    void Start()
    { 
        foreach (var step in cutsceneSteps)
        {
            step.canvasGroup = step.cutsceneObject.GetComponent<CanvasGroup>();
            step.cutsceneObject.SetActive(false);  
        }

        ShowStep(currentIndex);  
        cutsceneSteps[currentIndex].canvasGroup.alpha = 1f;
    }

    public void AdvanceCutscene()
    {
        if (currentIndex < cutsceneSteps.Length - 1)
        {
            StartCoroutine(TransitionToStep(currentIndex + 1));
        }
        else
        {
            SceneManager.LoadScene(nextSceneName);
        }
    }

    IEnumerator TransitionToStep(int nextIndex)
    {
        var currentStep = cutsceneSteps[currentIndex];
        var nextStep = cutsceneSteps[nextIndex];

        yield return StartCoroutine(FadeOut(currentStep.canvasGroup));
        currentStep.cutsceneObject.SetActive(false);

        currentIndex = nextIndex;
        ShowStep(currentIndex);

        yield return StartCoroutine(FadeIn(nextStep.canvasGroup));
    }

    void ShowStep(int index)
    {
        var step = cutsceneSteps[index];
        step.cutsceneObject.SetActive(true);
        step.canvasGroup.alpha = 0f; 
    }

    IEnumerator FadeIn(CanvasGroup group)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            group.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        group.alpha = 1f;
    }

    IEnumerator FadeOut(CanvasGroup group)
    {
        float t = 0f;
        while (t < fadeDuration)
        {
            group.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
            t += Time.deltaTime;
            yield return null;
        }
        group.alpha = 0f;
    }
}

