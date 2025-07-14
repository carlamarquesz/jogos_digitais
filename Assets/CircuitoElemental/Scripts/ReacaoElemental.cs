using UnityEngine;

[CreateAssetMenu(fileName = "NovaReacao", menuName = "Circuito/Reação Elemental")]
public class ReacaoElemental : ScriptableObject
{
    public TipoElemento elementoA;
    public TipoElemento elementoB;
    public TipoElemento resultado;
}
