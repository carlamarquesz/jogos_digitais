using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseUI;
    private bool isPaused = false;

    private PlayerControls inputActions; // Usando o nome correto do input actions

    void Awake()
    {
        inputActions = new PlayerControls(); // Instancia o mapa de controles

        // Escuta a a��o de pause
        inputActions.Player.Pause.performed += ctx => TogglePause();
    }

    void OnEnable()
    {
        inputActions.Enable(); // Habilita a��es quando o objeto � ativado
    }

    void OnDisable()
    {
        inputActions.Disable(); // Desabilita a��es quando o objeto � desativado
    }

    void TogglePause()
    {
        if (isPaused)
            ResumeGame();
        else
            PauseGame();
    }

    public void PauseGame()
    {
        if (pauseUI != null)
            pauseUI.SetActive(true);

        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame()
    {
        if (pauseUI != null)
            pauseUI.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
    }

    public void ExitToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MenuPrincipal"); // Substitua pelo nome da sua cena
    }
}
