using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 2f; 
     
    private Animator animator;
    private Rigidbody2D rb;
    private PlayerControls controls;
    public ParticleSystem rocketFlame;

    public GameObject projectilePrefab;     // Prefab do projétil
    public Transform shootPoint;            // Ponto de onde o projétil vai sair
    public Transform enemy;                // Referência ao inimigo
    private bool isBoosting = false;

    public float projectileSpeed = 5f;
    public int energyPiecesToRecharge = 3; // Quantidade necessária para recarregar
    private int energyCollected = 0;       // Quantas peças foram coletadas até agora


    private void Awake()
    {
        controls = new PlayerControls();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        controls.Enable();
        controls.Player.Jump.started += ctx =>
        {
            isBoosting = true;
        };

        controls.Player.Jump.canceled += ctx =>
        {
            isBoosting = false;
            GameManager.instance.PararBoost(); // <- Notifica que o boost parou
        };

        controls.Player.Shoot.performed += ctx => Shoot();


    }


    private void OnDisable()
    {
        controls.Player.Jump.started -= ctx => isBoosting = true;
        controls.Player.Jump.canceled -= ctx => isBoosting = false;
        controls.Player.Shoot.performed -= ctx => Shoot();
        controls.Disable();
    }


    private void Update()
    {
        Vector2 input = controls.Player.Move.ReadValue<Vector2>();

        // Direção normal de movimento
        Vector2 moveDirection = input.normalized * moveSpeed;

        // Aplique o impulso se botão está pressionado, há input E tem boost disponível
        if (isBoosting && input != Vector2.zero && GameManager.instance.TentarUsarBoost())
        {
            moveDirection += input.normalized * jumpForce;
        }

        rb.linearVelocity = moveDirection;

        // Partículas (opcionalmente pode ativar só quando isBoosting for true)
        var main = rocketFlame.main;
        if (input == Vector2.zero && !isBoosting)
        {
            if (main.startLifetime.constant != 1f)
            {
                main.startLifetime = 1f;
                rocketFlame.Clear();
                rocketFlame.Play();
            }
        }
        else
        {
            if (main.startLifetime.constant != 2f)
            {
                main.startLifetime = 2f;
                rocketFlame.Clear();
                rocketFlame.Play();
            }
        }

        // Flip horizontal da nave
        if (input.x > 0)
            transform.localScale = new Vector3(0.5f, 0.5f, 1);
        else if (input.x < 0)
            transform.localScale = new Vector3(-0.5f, 0.5f, 1);
    }

     


    private void Shoot()
    {
        GameObject projectile = Instantiate(projectilePrefab, shootPoint.position, shootPoint.rotation);
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();

        rb.linearVelocity = Vector2.up * projectileSpeed;
    }


    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemy hit!");
            // Aqui você pode chamar o método TakeDamage do jogador, se quiser
        }
        //if (other.CompareTag("EnergyPiece"))
        //{
        //    GameManager.instance.ColetarEnergia();
        //    Destroy(other.gameObject);
        //}

    } 
}
