using UnityEngine;

public class Vulnerabilidade : MonoBehaviour
{
    public TipoMagia[] fraquezas;

    public bool EhVulneravelA(TipoMagia tipo)
    {
        foreach (var f in fraquezas)
        {
            if (f == tipo) return true;
        }
        return false;
    }
}
