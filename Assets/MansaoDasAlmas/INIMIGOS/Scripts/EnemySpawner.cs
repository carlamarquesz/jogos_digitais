using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    [Header("Pontos de Spawn")]
    public Transform[] spawnPoints;

    [Header("Prefabs dos inimigos")]
    public GameObject[] enemyPrefabs;

    [Header("Configuração de Spawn")]
    public float initialInterval = 10f;
    public float fasterInterval = 5f;
    public float timeToSpeedUp = 30f;
    public int maxEnemies = 10;

    private Transform player;

    void Start()
    {
        player = GameObject.FindWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogWarning("Player não encontrado na cena! Certifique-se que ele tenha a tag 'Player'.");
        }
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
        if (spawnPoints.Length == 0 || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("SpawnPoints ou EnemyPrefabs não definidos!");
            return;
        }

        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

        GameObject enemy = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);

        EnemyController controller = enemy.GetComponent<EnemyController>();
        if (controller != null && player != null)
        {
            controller.player = player;
            float direction = (enemy.transform.position.x > player.position.x) ? -1f : 1f;
            controller.SetDirection(direction);
        }
    }

    int CountActiveEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
