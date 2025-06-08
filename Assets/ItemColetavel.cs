using UnityEngine;

public class ItemColetavel : MonoBehaviour
{
    public GameObject explosionEffect;
    public Spawner spawner; // Pode ser atribu�do manualmente OU via c�digo

    void Start()
    {
        Invoke(nameof(DestruirPorTempo), 3f); // 3s para sumir se n�o for tocado

        // Seguran�a: se spawner for null, tenta encontrar um na cena
        if (spawner == null)
        {
            spawner = FindObjectOfType<Spawner>();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.AddPoint();

            if (explosionEffect != null)
            {
                Instantiate(explosionEffect, transform.position, Quaternion.identity);
            }

            CancelInvoke(); // Evita destruir duas vezes
            Destroy(gameObject);

            // Chama novo spawn ap�s coleta
            spawner?.ChamarProximoSpawn();
        }
    }

    void DestruirPorTempo()
    {
        Destroy(gameObject);
        spawner?.ChamarProximoSpawn(); // Chama novo spawn mesmo que n�o tenha sido coletado
    }
}
