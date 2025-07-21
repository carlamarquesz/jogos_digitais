using UnityEngine;
using System.Collections.Generic;

public class DialogosManager : MonoBehaviour
{
    public static DialogosManager instance;
     
    private HashSet<string> dialogosConcluidos = new HashSet<string>();

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }
     
    public void MarcarDialogoComoConcluido(string id)
    {
        if (!dialogosConcluidos.Contains(id))
        {
            dialogosConcluidos.Add(id);
            Debug.Log("Diálogo marcado como concluído: " + id);
        }
    }
     
    public bool FoiConcluido(string id)
    {
        return dialogosConcluidos.Contains(id);
    }
}
