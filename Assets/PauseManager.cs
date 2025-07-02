using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    public GameObject pauseUI;
    private bool isPaused = false;

    private PlayerControls inputActions;  
    void Awake()
    {
        inputActions = new PlayerControls();   
        inputActions.Player.Pause.performed += ctx => TogglePause();
    }

    void OnEnable()
    {
        inputActions.Enable(); 
    }

    void OnDisable()
    {
        inputActions.Disable(); 
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
        SceneManager.LoadScene("MenuPrincipal");
    }
}
