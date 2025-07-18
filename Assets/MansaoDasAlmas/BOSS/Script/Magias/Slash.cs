using UnityEngine;

public class Slash : MonoBehaviour
{
    public float duration = 0.5f; // Tempo que o slash fica ativo
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator != null)
        {
            animator.Play("SlashAnimation");
        }

        // Destrói o objeto após o tempo da animação/duração
        Destroy(gameObject, duration);
    }

    // Opcional: detectar colisão com player para causar dano
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Aqui você pode chamar o método de dano do player
            Debug.Log("Player atingido pelo slash!");
        }
    }
}
