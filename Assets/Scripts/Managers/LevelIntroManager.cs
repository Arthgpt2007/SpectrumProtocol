
using System.Collections;
using UnityEngine;

[DefaultExecutionOrder(1000)]
public class LevelIntroManager : MonoBehaviour
{
    [Header("Briefing Panels")]
    public GameObject level1Briefing;
    public GameObject level2Briefing;

    public GameObject level3Briefing;

    [Header("Settings")]
    public bool showLevel1OnStart = true;

    public static bool BriefingOpen { get; private set; }

    public bool IsOpen => BriefingOpen;

    private GameObject currentPanel;

    private void Awake()
    {
        BriefingOpen = showLevel1OnStart;

        if (level1Briefing != null)
            level1Briefing.SetActive(false);

        if (level2Briefing != null)
            level2Briefing.SetActive(false);

        if (level3Briefing != null)
            level3Briefing.SetActive(false);
    }

    private IEnumerator Start()
    {
        if (showLevel1OnStart)
        {
            yield return null;
            ShowLevel1Briefing();
        }
    }

    public void ShowLevel1Briefing()
    {
        ShowBriefing(level1Briefing);
    }

    public void ShowLevel2Briefing()
    {
        ShowBriefing(level2Briefing);
    }

    public void ShowLevel3Briefing()
    {
        ShowBriefing(level3Briefing);
    }

    private void ShowBriefing(GameObject panel)
    {
        if (panel == null)
        {
            Debug.LogError("Briefing panel not assigned!");
            return;
        }

        if (currentPanel != null)
            currentPanel.SetActive(false);

        currentPanel = panel;

        BriefingOpen = true;
        panel.SetActive(true);

        Time.timeScale = 0f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseBriefing()
    {
        if (!BriefingOpen)
            return;

        if (currentPanel != null)
            currentPanel.SetActive(false);

        currentPanel = null;
        BriefingOpen = false;

        // Prevent the BEGIN click from firing the laser.
        ColorGun.blockShootingUntil =
            Time.unscaledTime + 0.3f;

        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDestroy()
    {
        BriefingOpen = false;
    }
}
