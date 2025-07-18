using UnityEngine;
using System.Collections;

public class BossController1 : MonoBehaviour
{
    public Transform player;
    public float dashSpeed = 8f;
    public float dashCooldown = 1f;
    private float lastDashTime;

    private Rigidbody2D rb;
    private bool isDashing = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        lastDashTime = -dashCooldown;
    }

    void Update()
    {
        if (player == null)
            return;

        float distance = Vector2.Distance(transform.position, player.position);
        bool podeDash = (Time.time - lastDashTime > dashCooldown);

        if (distance < 4f && podeDash && !isDashing)
            StartCoroutine(DashAttack());
    }

    IEnumerator DashAttack()
    {
        isDashing = true;
        lastDashTime = Time.time;

        Vector2 direction = (player.position - transform.position).normalized;
        float dashTime = 2f;
        float timer = 0;
        float minDistanceToStop = 0.7f;

        while (timer < dashTime)
        {
            float distanceToPlayer = Vector2.Distance(rb.position, player.position);

            if (distanceToPlayer <= minDistanceToStop)
                break;

            Vector2 newPosition = rb.position + direction * dashSpeed * Time.deltaTime;
            rb.MovePosition(newPosition);

            timer += Time.deltaTime;
            yield return null;
        }

        isDashing = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, 4f);
    }
}
