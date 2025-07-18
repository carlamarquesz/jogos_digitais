using UnityEngine;

public class Vulnerabilidade : MonoBehaviour
{
    public MagicType[] fraquezas;  // Troque TipoMagia por MagicType

    public bool EhVulneravelA(MagicType tipo)
    {
        foreach (var f in fraquezas)
        {
            if (f == tipo) return true;
        }
        return false;
    }
}
