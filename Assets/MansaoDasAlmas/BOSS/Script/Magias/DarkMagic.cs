using UnityEngine;

public class DarkMagic : MonoBehaviour
{
    public float speed = 5f;
    public int manaDrain = 10;
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
            var mana = other.GetComponent<PlayerMana>();
            if (mana != null)
            {
                mana.DrainMana(manaDrain);
            }
        }

        if (impactEffectPrefab)
            Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }
}
