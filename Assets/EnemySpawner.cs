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

    [Tooltip("Número máximo de inimigos ativos ao mesmo tempo")]
    public int maxEnemies = 10;
    [SerializeField] private float minY = -2f;
    [SerializeField] private float maxY = 2f;

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
        // Decide lateral
        bool spawnRight = Random.value > 0.5f;

        // Converte um valor fixo de X (fora da tela) e Y aleatório dentro de faixa controlada
        float yRandom = Random.Range(minY, maxY);

        // Obtém posição da câmera (usando Viewport ou posição absoluta)
        Vector3 spawnPos = spawnRight
            ? new Vector3(Camera.main.transform.position.x + 10f, yRandom, 0f)
            : new Vector3(Camera.main.transform.position.x - 10f, yRandom, 0f);

        GameObject enemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        EnemyController controller = enemy.GetComponent<EnemyController>();
        if (controller != null && GameObject.FindWithTag("Player") != null)
        {
            controller.player = GameObject.FindWithTag("Player").transform;
            controller.SetDirection(spawnRight ? -1f : 1f);
        }
    }



    int CountActiveEnemies()
    {
        return GameObject.FindGameObjectsWithTag("Enemy").Length;
    }
}
