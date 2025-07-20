using UnityEngine;
using System.Collections;

public class BossController : MonoBehaviour
{
    public Transform player;
    public float detectionRange = 4f;

    public MagicManager magicManager;

    public float walkSpeed = 1.5f;       // Velocidade devagar para andar
    public float dashSpeed = 8f;
    public float dashCooldown = 1f;
    private float lastDashTime;

    public GameObject slashPrefab;

    private Rigidbody2D rb;
    private bool isDashing = false;

    private TeleportSystem teleportSystem;
    private float tempoDesdeUltimoTeleporte = 0f;
    public float intervaloTeleporte = 10f;

    private Coroutine dashCoroutine;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastDashTime = -dashCooldown;
        teleportSystem = GetComponent<TeleportSystem>();
    }

    void Update()
    {
        if (player == null)
        {
            Debug.LogWarning("Player não está atribuído!");
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);
        bool podeDash = (Time.time - lastDashTime > dashCooldown);

        if (distance < detectionRange && podeDash && !isDashing)
        {
            dashCoroutine = StartCoroutine(DashAttack());
        }

        // Movimentação lenta em direção ao player, se não estiver dashing
        if (!isDashing)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            Vector2 novaPosicao = rb.position + direction * walkSpeed * Time.deltaTime;
            rb.MovePosition(novaPosicao);
        }

        if (!isDashing && magicManager != null && magicManager.CanCast())
        {
            magicManager.CastCurrentMagic();
        }

        tempoDesdeUltimoTeleporte += Time.deltaTime;

        if (tempoDesdeUltimoTeleporte >= intervaloTeleporte && !isDashing)
        {
            if (teleportSystem != null)
            {
                teleportSystem.Teleport();
                tempoDesdeUltimoTeleporte = 0f;
            }
        }
    }

    IEnumerator DashAttack()
    {
        isDashing = true;
        lastDashTime = Time.time;

        Vector2 direction = (player.position - transform.position).normalized;
        float dashTime = 2f;
        float timer = 0;
        float minDistanceToSlash = 0.7f;

        while (timer < dashTime)
        {
            float distanceToPlayer = Vector2.Distance(rb.position, player.position);

            if (distanceToPlayer <= minDistanceToSlash)
            {
                DispararSlash();
                break;
            }

            Vector2 newPosition = rb.position + direction * dashSpeed * Time.deltaTime;
            rb.MovePosition(newPosition);

            timer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    private void DispararSlash()
    {
        if (slashPrefab != null && player != null)
        {
            Vector3 playerPos = player.position;

            float lado = Mathf.Sign(playerPos.x - transform.position.x);

            float offsetX = 0.5f;

            Vector3 slashPos = playerPos + new Vector3(offsetX * lado, 0, 0);

            Quaternion rot = Quaternion.identity;
            if (lado < 0)
                rot = Quaternion.Euler(0, 180, 0);

            Instantiate(slashPrefab, slashPos, rot);
            Debug.Log($"Slash disparado na posição {slashPos} com lado {lado}");
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDashing && collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Colidiu com player! Parando dash e disparando slash.");

            DispararSlash();

            if (dashCoroutine != null)
            {
                StopCoroutine(dashCoroutine);
                dashCoroutine = null;
            }

            isDashing = false;
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
