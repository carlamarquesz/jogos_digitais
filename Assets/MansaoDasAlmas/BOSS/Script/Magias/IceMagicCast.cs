using UnityEngine;

public class IceMagicCast : MonoBehaviour
{
    public float speed = 5f;
    public Vector2 direction = Vector2.up;
    public int damage = 1;
    public float lifetime = 3f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(direction.normalized * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Aplicar dano aqui
            Debug.Log("Player atingido pela magia de gelo!");
            Destroy(gameObject);
        }
    }
}
