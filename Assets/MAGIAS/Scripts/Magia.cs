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
        if (other.CompareTag("Enemy"))
        {
            EnemyHealth vida = other.GetComponent<EnemyHealth>();
            Vulnerabilidade inimigoVuln = other.GetComponent<Vulnerabilidade>();

            if (vida != null)
            {
                // Só aplica slow se for gelo e inimigo vulnerável
                if (tipoMagia == MagicType.Ice && (inimigoVuln == null || inimigoVuln.EhVulneravelA(tipoMagia)))
                {
                    EnemySlow slow = other.GetComponent<EnemySlow>();
                    if (slow != null)
                    {
                        slow.AplicarLentidao(0.6f, 2f); // 60% slow por 2 segundos
                        Debug.Log("Slow aplicado no inimigo.");
                    }
                }

                if (inimigoVuln != null && !inimigoVuln.EhVulneravelA(tipoMagia))
                {
                    Debug.Log("Magia bloqueada pelo inimigo!");
                    Destroy(gameObject);
                    return;
                }

                vida.TakeDamage(tipoMagia);

                Destroy(gameObject);
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
