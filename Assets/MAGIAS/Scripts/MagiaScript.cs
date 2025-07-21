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
    if (magias.Length == 0 || playerHealth == null)
        return;

    int custo = (custoManaPorMagia != null && currentMagiaIndex < custoManaPorMagia.Length)
                ? custoManaPorMagia[currentMagiaIndex]
                : 10;

    if (playerHealth.currentMana < custo)
    {
        Debug.Log("Mana insuficiente para usar a magia!");
        return;
    }

    playerHealth.UseMana(custo);

    GameObject magiaObj = Instantiate(magias[currentMagiaIndex], transform.position, transform.rotation);

    Magia magiaScript = magiaObj.GetComponent<Magia>();
    if (magiaScript != null)
    {
        MagicType[] tiposMagia = new MagicType[] { MagicType.Darkness, MagicType.Fire, MagicType.Ice };
        magiaScript.tipoMagia = currentMagiaIndex < tiposMagia.Length ? tiposMagia[currentMagiaIndex] : MagicType.Fire;

        float directionX = transform.localScale.x > 0 ? 1f : -1f;
        magiaScript.direcao = new Vector2(directionX, 0f);
        magiaScript.velocidade = magiaScript.velocidade;

        Vector3 escala = magiaObj.transform.localScale;
        escala.x = Mathf.Abs(escala.x) * directionX;
        magiaObj.transform.localScale = escala;
    }
}


}
