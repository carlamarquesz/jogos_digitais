using UnityEngine;

public class CameraSeguidora : MonoBehaviour
{
    public Transform jogador;
    public Vector3 offset = new Vector3(0, 0, -10); // Z precisa ser -10 para 2D

    void LateUpdate()
    {
        if (jogador != null)
        {
            transform.position = jogador.position + offset;
        }
    }
}
