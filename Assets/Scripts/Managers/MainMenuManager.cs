
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public string gameplaySceneName = "Gameplay";

    void Start()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Spectrum Protocol...");
        Application.Quit();
    }
}
