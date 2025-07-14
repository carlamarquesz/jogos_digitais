using UnityEngine;

public class FluxoMagico : MonoBehaviour
{
    public void IniciarFluxo(Vector3 origem, Vector3 destino)
    {
        transform.position = origem;
        Vector3 direcao = (destino - origem).normalized;
        float distancia = Vector3.Distance(origem, destino);
        StartCoroutine(Mover(direcao, distancia));
    }

    private System.Collections.IEnumerator Mover(Vector3 direcao, float distancia)
    {
        float percorrido = 0;
        float velocidade = 5f;

        while (percorrido < distancia)
        {
            transform.Translate(direcao * velocidade * Time.deltaTime);
            percorrido += velocidade * Time.deltaTime;
            yield return null;
        }

        Destroy(gameObject);
    }
}
