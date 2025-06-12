using UnityEngine;
public enum TipoItem
{
    Ponto,
    Energia
}
public class ItemColetavel : MonoBehaviour
{
    public TipoItem tipoItem = TipoItem.Ponto; // Escolhido no Inspector
    public GameObject explosionEffect;
    public Spawner spawner;

    void Start()
    {
        Invoke(nameof(DestruirPorTempo), 3f);

        if (spawner == null)
            spawner = FindObjectOfType<Spawner>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // Efeito visual
        if (explosionEffect != null)
            Instantiate(explosionEffect, transform.position, Quaternion.identity);

        // Ação conforme o tipo
        switch (tipoItem)
        {
            case TipoItem.Ponto:
                GameManager.instance.AddPoint();
                break;

            case TipoItem.Energia:
                GameManager.instance.ColetarEnergia();
                break;
        }

        CancelInvoke();
        Destroy(gameObject);
        spawner?.ChamarProximoSpawn();
    }

    void DestruirPorTempo()
    {
        Destroy(gameObject);
        spawner?.ChamarProximoSpawn();
    }
}
