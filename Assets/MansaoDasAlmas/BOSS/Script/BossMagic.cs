using UnityEngine;

[CreateAssetMenu(fileName = "NovaMagia", menuName = "Boss/Magia")]
public class BossMagic : ScriptableObject
{
    public string magicName;
    public GameObject prefab;
    public float cooldown;

    public bool isIceMagic;  // <-- adiciona essa linha
}
