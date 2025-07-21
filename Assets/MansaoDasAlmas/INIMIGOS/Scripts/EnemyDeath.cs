using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    public RoomController roomController;
    private bool isDead = false;

    public void Die()
    {
        if (isDead) return;
        isDead = true;

        if (roomController != null)
            roomController.RegisterKill();

        Destroy(gameObject);
    }
}
