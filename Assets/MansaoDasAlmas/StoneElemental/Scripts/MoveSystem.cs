using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    public GameObject correctForm;
    private bool moving;
    private float startPosX;
    private float startPosY;
    private Vector3 resetPosition;
    private bool finish;

    private SpriteRenderer sr;
    private int originalSortingOrder;

   void Start()
{
    resetPosition = this.transform.position;
    sr = GetComponent<SpriteRenderer>();
    if (sr != null)
    {
        originalSortingOrder = sr.sortingOrder;
        Debug.Log($"{gameObject.name} sortingOrder original: {originalSortingOrder}");
    }
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

        // Traz a peça para frente durante o drag
        if (sr != null)
            sr.sortingOrder = 100;
    }

    void OnMouseUp()
{
    moving = false;

    if (correctForm != null && Vector2.Distance(this.transform.position, correctForm.transform.position) <= 0.5f)
    {
        this.transform.position = new Vector3(
            correctForm.transform.position.x,
            correctForm.transform.position.y,
            this.transform.position.z
        );
        finish = true;

        if (sr != null)
            sr.sortingOrder = 100;

        GameObject.Find("PointsHandler").GetComponent<WinScript>().AddPoint();
    }
    else
    {
        this.transform.position = resetPosition;

        if (sr != null)
            sr.sortingOrder = originalSortingOrder;
    }
}

}
