using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour
{
    public GameObject objetoParaSpawnar;
    public float tempoParaRespawn = 3f;

    private GameObject objetoAtual;

    void Start()
    {
        SpawnarObjeto();
    }

    public void SpawnarObjeto()
    {
        // Verifica se o objeto que vamos spawnar é do tipo Energia
        ItemColetavel coletavel = objetoParaSpawnar.GetComponent<ItemColetavel>();
        if (coletavel != null && coletavel.tipoItem == TipoItem.Energia)
        {
            // Se boost está cheio e energia também, não spawna
            if (GameManager.instance != null &&
                GameManager.instance.boostCargas >= 1 &&
                GameManager.instance.energiaColetada >= GameManager.instance.energiaPorCarga)
            {
                Debug.Log("Spawn de energia bloqueado: boost cheio e energia 3/3.");
                // Tenta novamente depois do delay
                StartCoroutine(SpawnAposDelay());
                return;
            }
        }

        // Gera uma posição aleatória visível na tela
        Camera cam = Camera.main;
        Vector2 screenMin = cam.ViewportToWorldPoint(new Vector2(0, 0));
        Vector2 screenMax = cam.ViewportToWorldPoint(new Vector2(1, 1));

        float x = Random.Range(screenMin.x, screenMax.x);
        float y = Random.Range(screenMin.y, screenMax.y);

        objetoAtual = Instantiate(objetoParaSpawnar, new Vector2(x, y), Quaternion.identity);

        // Diz ao item quem é o spawner (pra ele avisar quando sumir)
        objetoAtual.GetComponent<ItemColetavel>().spawner = this;
    }

    public void ChamarProximoSpawn()
    {
        StartCoroutine(SpawnAposDelay());
    }

    private IEnumerator SpawnAposDelay()
    {
        yield return new WaitForSeconds(tempoParaRespawn);
        SpawnarObjeto();
    }
}
