using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerRotationInput : MonoBehaviour
{
    private PlayerControls input; 
    private Vector2 mouseScreenPosition;
    private Camera cam;

    void Awake()
    {
        input = new PlayerControls();
    }

    void OnEnable()
    {
        input.Enable();
        input.Player.Look.performed += ctx => mouseScreenPosition = ctx.ReadValue<Vector2>();
    }

    void OnDisable()
    {
        input.Disable();
    }

    void Start()
    {
        cam = Camera.main;
    }

    void Update()
    {
        // Converte posi��o do mouse na tela para posi��o no mundo
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f;

        // Calcula dire��o e �ngulo
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Aplica rota��o no eixo Z
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
