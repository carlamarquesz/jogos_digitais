using UnityEngine;

public class Magia : MonoBehaviour
{
    public float velocidade = 5f;
    public Vector2 direcao = Vector2.right; // ✅ Essa linha é essencial
    public int dano = 1;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direcao.normalized * velocidade;

        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth vida = other.GetComponent<EnemyHealth>();
            if (vida != null)
            {
                vida.TakeDamage(dano);
            }

            Destroy(gameObject);
        }
    }
}
