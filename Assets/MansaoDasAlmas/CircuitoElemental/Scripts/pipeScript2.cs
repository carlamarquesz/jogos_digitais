using UnityEngine;

public class pipeScript2 : MonoBehaviour
{
    float[] rotations = { 0, 90, 180, 270 };

    public float[] CorrectRotation;

    [SerializeField]
    private bool isPlaced = false;

    private PiperGameManager2 gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<PiperGameManager2>();
        if (gameManager == null)
        {
            Debug.LogError("PiperGameManager2 não encontrado na cena!");
        }
    }

    private void Start()
    {
        int rand = Random.Range(0, rotations.Length);
        transform.eulerAngles = new Vector3(0, 0, rotations[rand]);

        if (IsAnyRotationCorrect(transform.eulerAngles.z))
        {
            isPlaced = true;
            if (gameManager != null)
            {
                gameManager.correctMove();
                Debug.Log("Pipe colocado corretamente no início!");
            }
        }
    }

    private void OnMouseDown()
    {
        transform.Rotate(new Vector3(0, 0, 90));

        float currentRotation = NormalizeAngle(transform.eulerAngles.z);
        bool nowCorrect = IsAnyRotationCorrect(currentRotation);

        if (nowCorrect && !isPlaced)
        {
            isPlaced = true;
            if (gameManager != null)
            {
                gameManager.correctMove();
            }
            Debug.Log("Correto!");
        }
        else if (!nowCorrect && isPlaced)
        {
            isPlaced = false;
            if (gameManager != null)
            {
                gameManager.incorrectMove();
            }
            Debug.Log("Errado.");
        }
    }

    public bool IsPlaced()
    {
        return isPlaced;
    }

    private bool IsAnyRotationCorrect(float currentAngle)
    {
        currentAngle = NormalizeAngle(currentAngle);
        foreach (float correct in CorrectRotation)
        {
            if (Mathf.Abs(currentAngle - NormalizeAngle(correct)) < 0.1f)
                return true;
        }
        return false;
    }

    private float NormalizeAngle(float angle)
    {
        angle = angle % 360f;
        if (angle < 0) angle += 360f;
        return angle;
    }
}
