
using UnityEngine;
using UnityEngine.SceneManagement;

public class FinishScreen : MonoBehaviour
{
    [Header("UI")]
    public GameObject finishPanel;

    [Header("Player Components")]
    public FPSController fpsController;
    public ColorGun colorGun;
    public PlayerInteraction playerInteraction;

    private bool gameFinished;

    public void ShowFinishScreen()
    {
        if (gameFinished) return;

        gameFinished = true;

        if (finishPanel != null)
            finishPanel.SetActive(true);

        // Disable player controls
        if (fpsController != null)
            fpsController.enabled = false;

        if (colorGun != null)
            colorGun.enabled = false;

        if (playerInteraction != null)
            playerInteraction.enabled = false;

        // Pause the game
        Time.timeScale = 0f;

        // Unlock mouse for buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayAgain()
    {
        Time.timeScale = 1f;

        SceneManager.LoadScene(
            SceneManager.GetActiveScene().name
        );
    }

    public void ReturnToMenu()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SceneManager.LoadScene("MainMenu");
    }
}
