using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configura��o do Spawner")]
    public GameObject enemyPrefab;

    [Tooltip("Intervalo inicial entre os spawns (em segundos)")]
    public float initialInterval = 10f;

    [Tooltip("Intervalo mais r�pido ap�s os 30s")]
    public float fasterInterval = 5f;

    [Tooltip("Tempo em segundos at� acelerar")]
    public float timeToSpeedUp = 30f;

    [Tooltip("N�mero m�ximo de inimigos ativos ao mesmo tempo")]
    public int maxEnemies = 10;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        float timer = 0f;

        while (true)
        {
            float currentInterval = (timer < timeToSpeedUp) ? initialInterval : fasterInterval;

            if (CountActiveEnemies() < maxEnemies)
            {
                SpawnEnemy();
            }
            yield return new WaitForSeconds(currentInterval);
            timer += currentInterval;
        }
    }

    void SpawnEnemy()
    {
        Vector3 viewportPos = new Vector3(Random.Range(0.5f, 1f), Random.value, 10f);
        Vector3 worldPos = Camera.main.ViewportToWorldPoint(viewportPos);
        worldPos.z = 0f;

        GameObject enemy = Instantiate(enemyPrefab, worldPos, Quaternion.identity);

        EnemyController controller = enemy.GetComponent<EnemyController>();
        if (controller != null && GameObject.FindWithTag("Player") != null)
        {
            controller.player = GameObject.FindWithTag("Player").transform;
        }
    }

    int CountActiveEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
