using UnityEngine;

public class WinScript : MonoBehaviour
{
    private int pointsToWin;
    private int currentPoints;
    public GameObject myGrimoire;

    void Start()
    {
        pointsToWin = myGrimoire.transform.childCount;
    }

    void Update()
    {
        if (currentPoints >= pointsToWin)
        {
            transform.GetChild(0).gameObject.SetActive(true); // ✅ Ativa o primeiro filho
        }
    }
    
    public void AddPoint()
    {
        currentPoints++; 
    }
}
