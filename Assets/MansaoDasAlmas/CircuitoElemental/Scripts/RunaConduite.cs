using UnityEngine;

public enum TipoElemento { Fogo, Agua, Ar, Terra, Gelo, Raio, Luz }

public class RunaConduite : MonoBehaviour
{
    public TipoElemento tipo;
    public Vector2Int gridPosicao;

    [Tooltip("Quais lados essa runa possui conexões (topo, baixo, esquerda, direita)")]
    public bool[] conexoes = new bool[4]; // 0 = Cima, 1 = Direita, 2 = Baixo, 3 = Esquerda

    public void Rotacionar()
    {
        transform.Rotate(0, 0, -90); // Roda no sentido horário
        bool temp = conexoes[3];
        conexoes[3] = conexoes[2];
        conexoes[2] = conexoes[1];
        conexoes[1] = conexoes[0];
        conexoes[0] = temp;
    }
}
