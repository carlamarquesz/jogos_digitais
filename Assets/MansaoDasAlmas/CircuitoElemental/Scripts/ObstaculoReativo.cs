using UnityEngine;

public class ObstaculoReativo : MonoBehaviour
{
    public TipoElemento reacaoNecessaria;

    public void Reagir(TipoElemento tipo)
    {
        if (tipo == reacaoNecessaria)
        {
            Destroy(gameObject);
            // Efeitos visuais ou som aqui
        }
    }
}
