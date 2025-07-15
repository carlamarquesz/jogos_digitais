using UnityEngine;
using UnityEngine.InputSystem;

public class MagiaScript : MonoBehaviour
{
    private PlayerControls controls;
    public MagiaUIController uiController;

    public GameObject[] magias; // Prefabs visuais da magia (ícones ou visuais de seleção)
    public int currentMagiaIndex = 0;



    void Awake()
    {
        controls = new PlayerControls();
    }

    void OnEnable()
    {
        controls.Enable();
        controls.Player.NextSpell.performed += ctx => ProximaMagia();
        controls.Player.PrevSpell.performed += ctx => MagiaAnterior();
    }

    void OnDisable()
    {
        controls.Player.NextSpell.performed -= ctx => ProximaMagia();
        controls.Player.PrevSpell.performed -= ctx => MagiaAnterior();
        controls.Disable();
    }

    void Start()
    {
        AtualizarMagiaAtual();
    }

    public GameObject GetMagiaAtual()
    {
        if (magias == null || magias.Length == 0)
            return null;

        return magias[currentMagiaIndex];
    }

    void ProximaMagia()
    {
        if (magias.Length == 0) return;

        magias[currentMagiaIndex].SetActive(false);
        currentMagiaIndex = (currentMagiaIndex + 1) % magias.Length;
        AtualizarMagiaAtual();
    }

    void MagiaAnterior()
    {
        if (magias.Length == 0) return;

        magias[currentMagiaIndex].SetActive(false);
        currentMagiaIndex--;
        if (currentMagiaIndex < 0) currentMagiaIndex = magias.Length - 1;
        AtualizarMagiaAtual();
    }

    void AtualizarMagiaAtual()
    {
        if (magias.Length == 0) return;

        magias[currentMagiaIndex].SetActive(true);

        if (uiController != null)
            uiController.AtualizarIconeSelecionado(currentMagiaIndex);
    }
}
