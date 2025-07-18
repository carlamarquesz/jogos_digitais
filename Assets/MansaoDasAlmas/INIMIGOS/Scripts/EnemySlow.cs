using UnityEngine;
using System.Collections;

[RequireComponent(typeof(SpriteRenderer))]
public class EnemySlow : MonoBehaviour
{
    [Header("Velocidade")]
    [Tooltip("Velocidade base do inimigo")]
    public float velocidadeOriginal = 1f;

    [Tooltip("Velocidade atual do inimigo (visível apenas para debug)")]
    [ReadOnlyInInspector] public float velocidadeAtual;

    [Tooltip("Indica se o inimigo está sob efeito de lentidão")]
    [ReadOnlyInInspector] public bool estaLento = false;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        velocidadeAtual = velocidadeOriginal;
    }

    public void AplicarLentidao(float percentualSlow, float duracao)
    {
        if (!estaLento)
            StartCoroutine(DelaySlow(percentualSlow, duracao));
    }

    private IEnumerator DelaySlow(float percentualSlow, float duracao)
    {
        estaLento = true;
        velocidadeAtual *= 1f - percentualSlow;

        if (spriteRenderer != null)
            spriteRenderer.color = Color.cyan;

        Debug.Log($"Lentidão aplicada: velocidade = {velocidadeAtual}");

        yield return new WaitForSeconds(duracao);

        velocidadeAtual = velocidadeOriginal;
        estaLento = false;

        if (spriteRenderer != null)
            spriteRenderer.color = Color.white;

        Debug.Log("Lentidão removida: velocidade restaurada.");
    }

    public float GetVelocidadeAtual()
    {
        return velocidadeAtual;
    }
}
