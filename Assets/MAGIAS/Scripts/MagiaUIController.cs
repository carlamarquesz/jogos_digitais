using UnityEngine;
using UnityEngine.UI;

public class MagiaUIController : MonoBehaviour
{
    public Image[] iconesMagias;
    public Color corSelecionada = Color.white;
    public Color corNormal = Color.gray;

    public void AtualizarIconeSelecionado(int indexSelecionado)
    {
        for (int i = 0; i < iconesMagias.Length; i++)
        {
            iconesMagias[i].color = (i == indexSelecionado) ? corSelecionada : corNormal;
        }
    }
}
