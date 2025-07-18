using UnityEngine;

public class MagicCaster : MonoBehaviour
{
    public GameObject iceMagicPrefab;
    public Transform feetPoint;  // arraste o FeetPoint no inspetor

    public void CastIceMagic()
    {
        Instantiate(iceMagicPrefab, feetPoint.position, Quaternion.identity);
    }
}
