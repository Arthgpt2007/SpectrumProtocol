
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class Level2Entry : MonoBehaviour
{
    [Header("References")]
    public HUDManager hudManager;
    public LevelIntroManager introManager;

    [Header("Level 2 Objective")]
    [TextArea]
    public string levelObjective =
        "Match your camouflage to hiding zones and evade security drones";

    private bool activated = false;

    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (activated || !other.CompareTag("Player"))
            return;

        activated = true;

        // Update the objective at the top of the HUD
        if (hudManager != null)
            hudManager.SetObjective(levelObjective);

        // Show Level 2 briefing
        if (introManager != null)
            introManager.ShowLevel2Briefing();

        Debug.Log("LEVEL 2 STARTED!");
    }
}
