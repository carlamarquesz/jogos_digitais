using UnityEngine;
using System.Collections;


public class MagicManager : MonoBehaviour
{   
    public GameObject iceMagicCirclePrefab; // Prefab do círculo animado
    public GameObject iceMagicPillarPrefab; // Seu IceMagicRise
    public BossMagic[] magias;
    public Transform castPoint;
    public Transform player;

    private Collider2D bossCollider;

    private int currentMagicIndex = 0;
    private float lastCastTime = -Mathf.Infinity;
    private float magicSwitchTime = 3f; // tempo para trocar a magia
    private float lastSwitchTime = 0f;

    public float castCooldown = 1f; // tempo entre cada cast

    void Awake()
    {
        bossCollider = GetComponent<Collider2D>();
        if (bossCollider == null)
            Debug.LogWarning("Collider2D do boss não encontrado no MagicManager!");
    }

    void Update()
    {
        // Atualiza a posição do castPoint conforme o lado do player
        if (player != null)
        {
            Vector3 dir = player.position - castPoint.position;
            if (dir.x > 0)
            {
                castPoint.localPosition = new Vector3(1f, 0f, 0f);
                castPoint.localRotation = Quaternion.identity;
            }
            else
            {
                castPoint.localPosition = new Vector3(-1f, 0f, 0f);
                castPoint.localRotation = Quaternion.Euler(0, 180f, 0);
            }
        }

        // Verifica se é hora de trocar a magia
        if (Time.time - lastSwitchTime >= magicSwitchTime)
        {
            currentMagicIndex = (currentMagicIndex + 1) % magias.Length;
            lastSwitchTime = Time.time;
        }

        // Cast automático (ou você pode chamar CastCurrentMagic de outro lugar, se preferir)
        if (CanCast())
        {
            CastCurrentMagic();
        }
    }

    public bool CanCast() => Time.time - lastCastTime >= castCooldown;

    public void CastCurrentMagic()
{
    if (magias == null || magias.Length == 0) return;

    BossMagic magia = magias[currentMagicIndex];  // Primeiro declara
    if (magia.prefab == null) return;

    Debug.Log("Casted magic: " + magia.prefab.name + " | isIceMagic: " + magia.isIceMagic);  // Depois usa

    if (magia.isIceMagic)
    {
        Vector3 spawnPos = player.position + new Vector3(0, -0.5f, 0);

        // Instancia o círculo e aguarda 1 segundo para instanciar o pilar
        GameObject circle = Instantiate(iceMagicCirclePrefab, spawnPos, Quaternion.identity);

        // Start coroutine para spawnar o pilar depois
        StartCoroutine(SpawnPillarAfterDelay(spawnPos, 1f));
    }
    else
    {
        // Spawn normal para as outras magias
        GameObject magiaObj = Instantiate(magia.prefab, castPoint.position, castPoint.rotation);

        Collider2D magiaCollider = magiaObj.GetComponent<Collider2D>();
        if (magiaCollider != null && bossCollider != null)
        {
            Physics2D.IgnoreCollision(magiaCollider, bossCollider);
        }

        MagicProjectile proj = magiaObj.GetComponent<MagicProjectile>();
        if (proj != null && player != null)
        {
            Vector2 directionToPlayer = (player.position - castPoint.position).normalized;
            proj.SetDirection(directionToPlayer);
        }
    }

    lastCastTime = Time.time;
}


    private IEnumerator SpawnPillarAfterDelay(Vector3 position, float delay)
    {
        yield return new WaitForSeconds(delay);

        GameObject pillar = Instantiate(iceMagicPillarPrefab, position, Quaternion.identity);
    }
}
