using UnityEngine;
using UnityEngine.EventSystems;

public class RunaInteracao : MonoBehaviour
{
    private Vector3 offset;
    private bool arrastando = false;
    private Camera cam;

    private void Start()
    {
        cam = Camera.main;
    }

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        offset = transform.position - GetMouseWorldPos();
        arrastando = true;
    }

    private void OnMouseUp()
    {
        arrastando = false;

        // Ajusta para a grade (opcional)
        transform.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), 0);

        // Atualiza posição no gerenciador
        GetComponent<RunaConduite>().gridPosicao = new Vector2Int((int)transform.position.x, (int)transform.position.y);
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) // Clique direito para rotacionar
        {
            GetComponent<RunaConduite>().Rotacionar();
        }
    }

    private void Update()
    {
        if (arrastando)
        {
            Vector3 mousePos = GetMouseWorldPos() + offset;
            transform.position = new Vector3(mousePos.x, mousePos.y, 0);
        }
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mouse = Input.mousePosition;
        mouse.z = 10f;
        return cam.ScreenToWorldPoint(mouse);
    }
}
