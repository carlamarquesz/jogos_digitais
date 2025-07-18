using UnityEngine;

public class TeleportSystem : MonoBehaviour
{
    public Transform[] teleportPoints;
    private float cooldown = 3f;
    private float lastTeleport;

    public bool ShouldTeleport() => Time.time - lastTeleport > cooldown;

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        lastTeleport = -cooldown;
    }

    public void Teleport()
    {
        if (teleportPoints == null || teleportPoints.Length == 0)
        {
            Debug.LogWarning("Nenhum ponto de teleporte definido!");
            return;
        }

        int index = Random.Range(0, teleportPoints.Length);

        if (rb != null)
        {
            rb.position = teleportPoints[index].position;
        }
        else
        {
            transform.position = teleportPoints[index].position;
        }

        lastTeleport = Time.time;
        Debug.Log($"Teleportando para ponto {index} em {teleportPoints[index].position}");
    }
}
