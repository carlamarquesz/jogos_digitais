using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject correctForm;
    private bool moving;
    private float startPosX;
    private float startPosY;
    private Vector3 resetPosition;
    private bool finish;

    void Start()
    {
        resetPosition = this.transform.position;
    }

    void Update()
    {
        if (!finish && moving)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);
            this.transform.position = new Vector3(
                mousePos.x - startPosX,
                mousePos.y - startPosY,
                this.transform.position.z
            );
        }
    }

    void OnMouseDown()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos = Camera.main.ScreenToWorldPoint(mousePos);

        startPosX = mousePos.x - this.transform.position.x;
        startPosY = mousePos.y - this.transform.position.y;
        moving = true;
    }

    void OnMouseUp()
    {
        moving = false;

        if (Vector2.Distance(this.transform.position, correctForm.transform.position) <= 0.5f)
        {
            this.transform.position = new Vector3(
                correctForm.transform.position.x,
                correctForm.transform.position.y,
                this.transform.position.z
            );
            finish = true;
            GameObject.Find("PointsHandler").GetComponent<WinScript>().AddPoint();
        }
        else
        {
            this.transform.position = resetPosition;
        }
    }
}
