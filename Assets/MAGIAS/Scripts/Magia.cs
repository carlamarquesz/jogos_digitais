using UnityEngine;

public enum TipoMagia { Fogo, Gelo, Trevas, Luz, Vento }

public class Magia : MonoBehaviour
{
    public TipoMagia tipoMagia = TipoMagia.Fogo;

    public float velocidade = 5f;
    public Vector2 direcao = Vector2.right;
    public int dano = 1;

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

            if (vida != null && inimigoVuln != null)
            {
                if (inimigoVuln.EhVulneravelA(tipoMagia))
                {
                    vida.TakeDamage(dano);
                    Destroy(gameObject);
                }
                else
                {
                    // Se quiser, toque som ou efeito de bloqueio aqui
                    Debug.Log("Magia bloqueada pelo inimigo!");
                    Destroy(gameObject);
                }
            }
            else
            {
                // Caso o inimigo não tenha script de vulnerabilidade, aplica dano normal
                if (vida != null)
                {
                    vida.TakeDamage(dano);
                    Destroy(gameObject);
                }
            }
        }
        else //if (other.CompareTag("Obstacle"))
        {
            // Destrói magia ao bater em obstáculo
            Destroy(gameObject);
        }
    }
}
