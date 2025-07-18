using UnityEngine;

public class WindMagic : MonoBehaviour
{
    public float speed = 6f;
    public float pushForce = 5f;
    public GameObject impactEffectPrefab;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Vector2 dir = (GameObject.FindGameObjectWithTag("Player").transform.position - transform.position).normalized;
        rb.linearVelocity = dir * speed;
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                Vector2 pushDir = (other.transform.position - transform.position).normalized;
                playerRb.AddForce(pushDir * pushForce, ForceMode2D.Impulse);
            }
        }

        if (impactEffectPrefab)
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
