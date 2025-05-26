using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Configuração do Spawner")]
    public GameObject enemyPrefab;

    [Tooltip("Intervalo inicial entre os spawns (em segundos)")]
    public float initialInterval = 10f;

    [Tooltip("Intervalo mais rápido após os 30s")]
    public float fasterInterval = 5f;

    [Tooltip("Tempo em segundos até acelerar")]
    public float timeToSpeedUp = 30f;

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

            SpawnEnemy();

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
}
