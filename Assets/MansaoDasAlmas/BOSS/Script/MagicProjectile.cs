using UnityEngine;

public class MagicProjectile : MonoBehaviour
{
    public float speed = 5f;
    private Vector2 direction = Vector2.right; // direção padrão

    public void SetDirection(Vector2 dir)
    {
        direction = dir.normalized;

        // Ajusta a rotação para olhar para a direção do movimento (opcional)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Update()
    {
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Sua lógica de colisão aqui
        Destroy(gameObject);
    }
}
