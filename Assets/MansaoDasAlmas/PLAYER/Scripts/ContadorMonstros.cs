using UnityEngine;

public class ContadorMonstros : MonoBehaviour
{
    public static int monstrosDerrotados = 0;

    public static void IncrementarContador()
    {
        monstrosDerrotados++;
    }
}
