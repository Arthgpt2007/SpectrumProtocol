
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDManager : MonoBehaviour
{

    [Header("Interaction")]
    public PlayerInteraction playerInteraction;

    [Header("Gameplay References")]
    public ColorGun colorGun;
    public HSVPlayer hsvPlayer;
    public SecurityDrone[] drones;

    [Header("Text Elements")]
    public TMP_Text objectiveText;
    public TMP_Text crosshairText;
    public TMP_Text interactionText;
    public TMP_Text camouflageStatus;
    public TMP_Text gunColorText;
    public TMP_Text detectionWarning;

    [Header("Colour Previews")]
    public Image camouflageColorPreview;
    public Image gunColorPreview;

    [Header("Objective")]
    [TextArea]
    public string currentObjective =
        "Restore the laboratory systems";

    void Start()
    {
        if (crosshairText != null)
            crosshairText.text = "+";
    }

    void Update()
    {
        UpdateGunUI();
        UpdateCamouflageUI();
        UpdateDroneWarning();
        UpdateObjective();
        if (interactionText != null && playerInteraction != null)
        {
            interactionText.text =
                playerInteraction.InteractionPrompt;
        }
    }

    void UpdateGunUI()
    {
        if (colorGun == null) return;

        CombatColor selected = colorGun.selectedColor;

        Color displayColor =
            CombatColorUtility.ToUnityColor(selected);

        if (gunColorText != null)
        {
            gunColorText.text =
                "ENERGY: " + selected.ToString().ToUpper();
            gunColorText.color = displayColor;
        }

        if (gunColorPreview != null)
            gunColorPreview.color = displayColor;
    }

    void UpdateCamouflageUI()
    {
        if (hsvPlayer == null) return;

        bool hidden = hsvPlayer.IsHidden;

        if (camouflageStatus != null)
        {
            string presetName = "None";

            if (hsvPlayer.presets != null &&
                hsvPlayer.presets.Length > 0)
            {
                int index = Mathf.Clamp(
                    hsvPlayer.currentPreset,
                    0,
                    hsvPlayer.presets.Length - 1
                );

                presetName =
                    hsvPlayer.presets[index].presetName;
            }

            camouflageStatus.text =
                "CAMOUFLAGE: " + presetName +
                "\n" + (hidden ? "HIDDEN" : "EXPOSED");

            camouflageStatus.color =
                hidden ? Color.green : Color.white;
        }

        if (camouflageColorPreview != null)
            camouflageColorPreview.color =
                hsvPlayer.CurrentColor;
    }

    void UpdateDroneWarning()
    {
        if (detectionWarning == null) return;

        bool detected = false;

        if (drones != null)
        {
            foreach (SecurityDrone drone in drones)
            {
                if (drone != null &&
                    drone.isActiveAndEnabled &&
                    drone.currentState ==
                        SecurityDrone.DroneState.Chase)
                {
                    detected = true;
                    break;
                }
            }
        }

        detectionWarning.text =
            detected ? "SECURITY ALERT - DETECTED" : "";

        detectionWarning.color = Color.red;
    }

    void UpdateObjective()
    {
        if (objectiveText != null)
            objectiveText.text =
                "OBJECTIVE: " + currentObjective;
    }

    public void SetObjective(string newObjective)
    {
        currentObjective = newObjective;
    }
}
