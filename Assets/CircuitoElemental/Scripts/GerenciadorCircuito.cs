using UnityEngine;
using System.Collections.Generic;

public class GerenciadorCircuito : MonoBehaviour
{
    public Vector2Int pontoA;
    public Vector2Int pontoB;

    private Dictionary<Vector2Int, RunaConduite> gridRunas = new Dictionary<Vector2Int, RunaConduite>();

    public List<ReacaoElemental> reacoes;

    public void RegistrarRuna(RunaConduite runa)
    {
        gridRunas[runa.gridPosicao] = runa;
    }

    public void VerificarCaminho()
    {
        List<Vector2Int> visitados = new();
        bool caminhoValido = Explorar(pontoA, visitados, null);

        if (caminhoValido)
        {
            Debug.Log("Energia chegou até o ponto B!");
        }
        else
        {
            Debug.Log("Caminho interrompido.");
        }
    }

    private bool Explorar(Vector2Int pos, List<Vector2Int> visitados, RunaConduite anterior)
    {
        if (!gridRunas.ContainsKey(pos) || visitados.Contains(pos)) return false;

        RunaConduite atual = gridRunas[pos];
        visitados.Add(pos);

        if (pos == pontoB) return true;

        Vector2Int[] direcoes = { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };

        for (int i = 0; i < 4; i++)
        {
            if (!atual.conexoes[i]) continue;

            Vector2Int novaPos = pos + direcoes[i];

            if (!gridRunas.ContainsKey(novaPos)) continue;

            RunaConduite vizinha = gridRunas[novaPos];

            // Verifica conexão mútua
            int oposto = (i + 2) % 4;
            if (!vizinha.conexoes[oposto]) continue;

            // Verifica reações entre tipos
            TipoElemento resultado = ChecarReacao(atual.tipo, vizinha.tipo);
            if (resultado == TipoElemento.Fogo) // Exemplo de reação que causa explosão
            {
                Vector3 meio = (new Vector3(pos.x, pos.y) + new Vector3(novaPos.x, novaPos.y)) / 2f;
                Collider2D hit = Physics2D.OverlapCircle(meio, 0.4f);

                if (hit != null)
                {
                    var obstaculo = hit.GetComponent<ObstaculoReativo>();
                    if (obstaculo != null)
                    {
                        obstaculo.Reagir(resultado); // Destroi obstáculo
                        Debug.Log("Obstáculo destruído!");
                    }
                }
            }

        }

        return false;
    }

    public void ReiniciarCircuito()
    {
        foreach (var r in gridRunas.Values)
        {
            Destroy(r.gameObject); // ou mover para origem
        }
        gridRunas.Clear();
        FindObjectOfType<GeradorPuzzle>().Gerar();
    }


    private TipoElemento ChecarReacao(TipoElemento a, TipoElemento b)
    {
        foreach (var reacao in reacoes)
        {
            if ((reacao.elementoA == a && reacao.elementoB == b) || (reacao.elementoA == b && reacao.elementoB == a))
            {
                return reacao.resultado;
            }
        }
        return a; // Se não houver reação, retorna o próprio
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(new Vector3(pontoA.x, pontoA.y, 0), 0.2f);

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(new Vector3(pontoB.x, pontoB.y, 0), 0.2f);
    }

}

