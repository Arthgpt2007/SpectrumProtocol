
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{
    [Header("UI")]
    public GameObject pausePanel;

    [Header("Scene Names")]
    public string mainMenuScene = "MainMenu";

    public LevelIntroManager levelIntroManager;

    private bool isPaused;

    void Start()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        if (!LevelIntroManager.BriefingOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (LevelIntroManager.BriefingOpen)
            return;

        if (levelIntroManager != null && levelIntroManager.IsOpen)
            return;


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        if (isPaused) return;

        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null)
            pausePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void ResumeGame()
    {
        if (!isPaused) return;

        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null)
            pausePanel.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene(mainMenuScene);
    }
}
