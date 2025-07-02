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
        Vector3 mouseWorldPosition = cam.ScreenToWorldPoint(mouseScreenPosition);
        mouseWorldPosition.z = 0f; 
        Vector2 direction = (mouseWorldPosition - transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; 
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
