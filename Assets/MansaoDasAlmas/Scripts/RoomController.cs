using UnityEngine;
using System.Collections;

public class RoomController : MonoBehaviour
{
    [Header("Configuração de Spawn")]
    public GameObject[] enemyTypes;
    public Transform[] spawnPoints;
    public int enemiesToSpawn = 5;
    public float spawnDelay = 2f;

    [Header("Lógica da Sala")]
    public GameObject doorBlocker;
    public GameObject door;
    public int enemiesToDefeat = 5;

    private int enemiesDefeated = 0;
    private bool playerInside = false;
    private Collider2D doorCollider;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!playerInside && other.CompareTag("Player"))
        {
            playerInside = true;
            StartCoroutine(SpawnEnemiesGradually());

            if (doorBlocker != null)
                doorBlocker.SetActive(true);

            if (door != null)
            {
                door.SetActive(false); // Esconde visualmente
                doorCollider = door.GetComponent<Collider2D>();
                if (doorCollider != null)
                    doorCollider.enabled = false; // Desativa o trigger da porta
            }

            SpriteRenderer parentSprite = GetComponent<SpriteRenderer>();
            if (parentSprite != null)
                parentSprite.enabled = false;
        }
    }

    IEnumerator SpawnEnemiesGradually()
    {
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            Transform spawnPoint = spawnPoints[i % spawnPoints.Length];
            GameObject randomEnemy = enemyTypes[Random.Range(0, enemyTypes.Length)];
            GameObject enemy = Instantiate(randomEnemy, spawnPoint.position, Quaternion.identity);

            EnemyDeath deathScript = enemy.GetComponent<EnemyDeath>();
            if (deathScript != null)
                deathScript.roomController = this;

            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void RegisterKill()
    {
        enemiesDefeated++;
        Debug.Log("Inimigo derrotado. Total: " + enemiesDefeated);

        if (enemiesDefeated >= enemiesToDefeat)
        {
            Debug.Log("Todos os inimigos foram derrotados!");

            if (doorBlocker != null)
            {
                doorBlocker.SetActive(false);
                Debug.Log("Barreira desativada!");
            }

            if (door != null)
            {
                door.SetActive(true);
                if (doorCollider != null)
                    doorCollider.enabled = true; // Agora o trigger pode funcionar
                Debug.Log("Porta ativada!");
            }
        }
    }
}
