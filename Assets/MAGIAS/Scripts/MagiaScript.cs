using UnityEngine;
using UnityEngine.InputSystem;

public class MagiaScript : MonoBehaviour
{
    private PlayerControls controls;
    public MagiaUIController uiController;

    public GameObject[] magias;            // Prefabs/objetos das magias visuais
    public int[] custoManaPorMagia;        // Custos de mana por magia, índice correspondente
    public int currentMagiaIndex = 0;

    public PlayerHealth playerHealth;

    void Awake()
    {
        controls = new PlayerControls();
    }
    private System.Action<InputAction.CallbackContext> shootCallback;

    void OnEnable()
{
    controls.Enable();

    shootCallback = ctx => {
        Debug.Log("Shoot acionado!");
        TentarUsarMagia();
    };

    controls.Player.Shoot.performed += shootCallback;

    controls.Player.NextSpell.performed += ctx => ProximaMagia();
    controls.Player.PrevSpell.performed += ctx => MagiaAnterior();
}

void OnDisable()
{
    controls.Player.Shoot.performed -= shootCallback;

    controls.Player.NextSpell.performed -= ctx => ProximaMagia();
    controls.Player.PrevSpell.performed -= ctx => MagiaAnterior();

    controls.Disable();
}
    void Start()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        if (playerHealth == null)
        {
            Debug.LogError("PlayerHealth não encontrado!");
        }

        AtualizarMagiaAtual();
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

        // Ativa a magia atual e desativa as outras
        for (int i = 0; i < magias.Length; i++)
        {
            magias[i].SetActive(i == currentMagiaIndex);
        }

        if (uiController != null)
            uiController.AtualizarIconeSelecionado(currentMagiaIndex);
    }

    void TentarUsarMagia()
{
    Debug.Log("Tentando usar magia...");
    
    if (magias.Length == 0)
    {
        Debug.LogWarning("Nenhuma magia configurada!");
        return;
    }

    if (playerHealth == null)
    {
        Debug.LogWarning("playerHealth é null!");
        return;
    }

    int custo = (custoManaPorMagia != null && currentMagiaIndex < custoManaPorMagia.Length)
                ? custoManaPorMagia[currentMagiaIndex]
                : 10;

    Debug.Log($"Mana atual: {playerHealth.currentMana}, custo magia: {custo}");

    if (playerHealth.currentMana >= custo)
    {
        playerHealth.UseMana(custo);
        Debug.Log($"Magia {currentMagiaIndex} usada! Mana restante: {playerHealth.currentMana}");

        // Instanciar magia se quiser:
        // Instantiate(magias[currentMagiaIndex], transform.position, Quaternion.identity);
    }
    else
    {
        Debug.Log("Mana insuficiente para usar a magia!");
    }
}

}
