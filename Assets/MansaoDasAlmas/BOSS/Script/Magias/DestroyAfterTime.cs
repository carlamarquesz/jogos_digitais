using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    public float timeToDestroy = 0.5f;
    void Start() => Destroy(gameObject, timeToDestroy);
}
