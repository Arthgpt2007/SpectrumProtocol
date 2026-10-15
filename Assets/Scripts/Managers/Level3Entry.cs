
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Level3Entry : MonoBehaviour
{
    [Header("References")]
    public HUDManager hudManager;
    public LevelIntroManager introManager;

    [Header("Level 3 Objective")]
    [TextArea]
    public string levelObjective =
        "Restore the central reactor";

    private bool activated = false;

    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated || !other.CompareTag("Player"))
            return;

        if (hudManager == null || introManager == null)
        {
            Debug.LogError(
                "Level 3: HUD Manager or Intro Manager missing!"
            );
            return;
        }

        activated = true;

        // Change objective at top of screen
        hudManager.SetObjective(levelObjective);

        // Display Level 3 briefing
        introManager.ShowLevel3Briefing();

        Debug.Log("LEVEL 3 STARTED!");
    }
}
