using UnityEngine;

public class BossIceSpellCast : MonoBehaviour
{
    public GameObject iceMagicPrefab;
    public float delayBeforeCast = 1f;

    void Start()
    {
        Invoke(nameof(CastIceSpell), delayBeforeCast);
    }

    void CastIceSpell()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && iceMagicPrefab != null)
        {
            Vector3 spawnPosition = player.transform.position; // pé do jogador
            Instantiate(iceMagicPrefab, spawnPosition, Quaternion.identity);
        }
    }
}
