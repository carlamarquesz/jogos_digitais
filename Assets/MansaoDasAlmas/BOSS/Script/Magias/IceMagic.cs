using UnityEngine;

public class IceMagicRise : MonoBehaviour
{
    public float damage = 10f;
    public float riseAmount = 1f;
    public float riseDuration = 0.5f;
    public float slowDuration = 2f;
    public float slowAmount = 0.5f;
    public GameObject impactEffectPrefab;

    public float activationDelay = 1.0f; // tempo para ativar dano (delay)

    private bool hasActivated = false;
    private Vector3 startPos;
    private Vector3 endPos;
    private float timer = 0f;

    private Collider2D col;

    void Start()
    {
        startPos = transform.position;
        endPos = startPos + new Vector3(0, riseAmount, 0);

        col = GetComponent<Collider2D>();
        if (col != null)
            col.enabled = false;  // desativa colisor no começo para não causar dano
    }

    void Update()
    {
        if (!hasActivated)
        {
            timer += Time.deltaTime;

            if (timer >= activationDelay)
            {
                hasActivated = true;
                if (col != null)
                    col.enabled = true;  // ativa colisor para causar dano

                timer = 0f; // reset timer para usar no movimento
            }
        }
        else
        {
            // movimento da magia subindo (como antes)
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / riseDuration);
            transform.position = Vector3.Lerp(startPos, endPos, t);

            if (t >= 1f)
            {
                Destroy(gameObject, 1.5f);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!hasActivated)
            return;  // não causa dano antes do delay

        if (other.CompareTag("Magic") || other.CompareTag("BossMagic"))
            return;

        if (other.CompareTag("Player"))
        {
            var movement = other.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                movement.ModifySpeedCumulative(0.2f, slowDuration);
            }

            var player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage((int)damage);
            }

            if (impactEffectPrefab)
                Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);

            Destroy(gameObject);
        }
        // else if (other.CompareTag("Obstacle"))
        // {
        //     if (impactEffectPrefab)
        //         Instantiate(impactEffectPrefab, transform.position, Quaternion.identity);

        //     Destroy(gameObject);
        // }
    }
}
