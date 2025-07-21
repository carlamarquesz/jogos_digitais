using UnityEngine;

public class GeradorPuzzle : MonoBehaviour
{
    public GameObject[] runasPrefabs;
    public int largura = 6;
    public int altura = 6;

    public void Gerar()
    {
        for (int x = 0; x < largura; x++)
        {
            for (int y = 0; y < altura; y++)
            {
                if (Random.value > 0.6f) continue;

                GameObject prefab = runasPrefabs[Random.Range(0, runasPrefabs.Length)];
                Vector3 pos = new Vector3(x, y, 0);
                GameObject r = Instantiate(prefab, pos, Quaternion.identity);
                RunaConduite condu = r.GetComponent<RunaConduite>();
                condu.gridPosicao = new Vector2Int(x, y);

                FindObjectOfType<GerenciadorCircuito>().RegistrarRuna(condu);
            }
        }
    }
}

