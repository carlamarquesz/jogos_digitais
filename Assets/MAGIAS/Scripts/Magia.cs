using UnityEngine;

public class Magia : MonoBehaviour
{
    public MagicType tipoMagia = MagicType.Fire;
    public bool tipoDefinidoExterno = false;

    public float velocidade = 5f;
    public Vector2 direcao = Vector2.right;

    [Header("Efeitos visuais e sons")]
    public ParticleSystem efeitoParticulas;
    public AudioClip somDisparo;
    private AudioSource audioSource;

    private Rigidbody2D rb;
    private bool jaCausouDano = false;

    void Start()
    {
        if (!tipoDefinidoExterno)
            tipoMagia = MagicType.Fire;

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
        if (jaCausouDano)
            return;

        Debug.Log($"[{gameObject.name}] Colidiu com {other.name}, tag: {other.tag}, jaCausouDano: {jaCausouDano}");

        if (other.CompareTag("Enemy") || other.CompareTag("Boss"))
        {
            var vidaEnemy = other.GetComponent<EnemyHealth>();
            var bossHealth = other.GetComponent<BossHealth>();
            if (bossHealth == null)
                bossHealth = other.GetComponentInParent<BossHealth>();

            // Comentado: controle de vulnerabilidade temporariamente desativado
            /*
            var vulnerabilidade = other.GetComponent<Vulnerabilidade>();
            if (vulnerabilidade == null)
                vulnerabilidade = other.GetComponentInParent<Vulnerabilidade>();

            if (vulnerabilidade != null && !vulnerabilidade.EhVulneravelA(tipoMagia))
            {
                Debug.Log($"[{gameObject.name}] Magia {tipoMagia} bloqueada por {other.name}");
                FinalizarMagia();
                return;
            }
            */

            if (vidaEnemy != null)
            {
                if (tipoMagia == MagicType.Ice)
                {
                    EnemySlow slow = other.GetComponent<EnemySlow>();
                    if (slow != null)
                        slow.AplicarLentidao(0.6f, 2f);
                }

                vidaEnemy.TakeDamage(tipoMagia);
                Debug.Log($"[{gameObject.name}] Dano causado ao inimigo {other.name}");
                FinalizarMagia();
                return;
            }

            if (bossHealth != null)
            {
                bossHealth.TakeDamage(tipoMagia);
                Debug.Log($"[{gameObject.name}] Dano causado ao chefe {other.name}");
                FinalizarMagia();
                return;
            }
            else
            {
                Debug.LogWarning($"[{gameObject.name}] Não encontrou BossHealth em {other.name} nem nos pais/filhos.");
            }
        }

        FinalizarMagia(); // bateu em parede ou outro objeto
    }

    private void FinalizarMagia()
    {
        jaCausouDano = true;
        Destroy(gameObject);
    }
}
