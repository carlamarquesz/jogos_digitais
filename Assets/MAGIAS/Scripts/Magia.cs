using UnityEngine;

public class Magia : MonoBehaviour
{
    public MagicType tipoMagia = MagicType.Fire;
    public float velocidade = 5f;
    public Vector2 direcao = Vector2.right;

    [Header("Efeitos visuais e sons")]
    public ParticleSystem efeitoParticulas;
    public AudioClip somDisparo;
    private AudioSource audioSource;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.linearVelocity = direcao.normalized * velocidade;

        if (efeitoParticulas != null)
            efeitoParticulas.Play();

        audioSource = GetComponent<AudioSource>();
        if (audioSource != null && somDisparo != null)
            audioSource.PlayOneShot(somDisparo);

        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            // --- CHECAR ENEMY NORMAL ---
            var vidaEnemy = other.GetComponent<EnemyHealth>();
            var vulnerabilidade = other.GetComponent<Vulnerabilidade>();

            if (vidaEnemy != null)
            {
                if (tipoMagia == MagicType.Ice && (vulnerabilidade == null || vulnerabilidade.EhVulneravelA(tipoMagia)))
                {
                    EnemySlow slow = other.GetComponent<EnemySlow>();
                    if (slow != null)
                        slow.AplicarLentidao(0.6f, 2f);
                }

                if (vulnerabilidade != null && !vulnerabilidade.EhVulneravelA(tipoMagia))
                {
                    Debug.Log("Magia bloqueada pelo inimigo!");
                    Destroy(gameObject);
                    return;
                }

                vidaEnemy.TakeDamage(tipoMagia);
                Destroy(gameObject);
                return;
            }

            // --- CHECAR BOSS ---
            var bossHealth = other.GetComponent<BossHealth>();
            if (bossHealth != null)
            {
                bossHealth.TakeDamage(tipoMagia);
                Destroy(gameObject);
                return;
            }

            // Se chegou até aqui, destrua de qualquer forma
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
