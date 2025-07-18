using System.Collections;
using UnityEngine;

public class MagicBarrier : MonoBehaviour
{
    private Coroutine blinkRoutine;
    public int blinkCount = 3;
    public float blinkDuration = 0.1f;

    public enum ElementType { Fire, Ice, Dark, Light, Wind }
    public ElementType currentElement;

    public float changeInterval = 10f;
    private float timer;

    [Header("Referências")]
    public ParticleSystem barrierEffect;

    [Header("Cores por Elemento")]
    public Color fireColor, iceColor, darkColor, lightColor, windColor;

    void Awake()
    {
        if (barrierEffect == null)
        {
            barrierEffect = GetComponentInChildren<ParticleSystem>();
            if (barrierEffect == null)
            {
                Debug.LogError("⚠️ ParticleSystem 'barrierEffect' não atribuído no MagicBarrier! Atribua no Inspector ou adicione um como filho.");
            }
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer > changeInterval)
        {
            ChangeElement();
            timer = 0;
        }
    }

    void Start()
{
    if (barrierEffect != null)
    {
        Debug.Log("Tocando ParticleSystem da barreira");
        barrierEffect.Play();
    }
}

    public void ChangeElement()
    {
        currentElement = (ElementType)Random.Range(0, 5);
        Debug.Log("Nova barreira: " + currentElement);
        UpdateVisualEffect();
    }

    void UpdateVisualEffect()
    {
        if (barrierEffect == null) return;

        if (blinkRoutine != null)
            StopCoroutine(blinkRoutine);

        blinkRoutine = StartCoroutine(BlinkEffect());
    }

    public bool CanTakeDamageFrom(ElementType element)
    {
        return element == currentElement;
    }

    private IEnumerator BlinkEffect()
    {
        if (barrierEffect == null) yield break;

        var main = barrierEffect.main;
        ParticleSystem.MinMaxGradient finalColor = currentElement switch
        {
            ElementType.Fire => new ParticleSystem.MinMaxGradient(fireColor),
            ElementType.Ice => new ParticleSystem.MinMaxGradient(iceColor),
            ElementType.Dark => new ParticleSystem.MinMaxGradient(darkColor),
            ElementType.Light => new ParticleSystem.MinMaxGradient(lightColor),
            ElementType.Wind => new ParticleSystem.MinMaxGradient(windColor),
            _ => main.startColor
        };

        for (int i = 0; i < blinkCount; i++)
        {
            main.startColor = new Color(0, 0, 0, 0); // Transparente
            yield return new WaitForSeconds(blinkDuration);
            main.startColor = finalColor;
            yield return new WaitForSeconds(blinkDuration);
        }

        main.startColor = finalColor;
        barrierEffect.Play();
    }
}
