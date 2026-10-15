
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Camera playerCamera;
    public float interactionDistance = 3f;
    public LayerMask interactionLayers = ~0;

    private IInteractable currentInteractable;

    public string InteractionPrompt =>
    currentInteractable != null
        ? "[E] " + currentInteractable.GetInteractionText()
        : "";

    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        currentInteractable = null;

        Ray ray = new Ray(
            playerCamera.transform.position,
            playerCamera.transform.forward
        );

        if (Physics.Raycast(
            ray,
            out RaycastHit hit,
            interactionDistance,
            interactionLayers))
        {
            currentInteractable =
                hit.collider.GetComponentInParent<IInteractable>();
        }

        if (currentInteractable != null)
        {
            if (Input.GetKeyDown(KeyCode.E))
                currentInteractable.Interact();
        }
    }

    /*void OnGUI()
    {
        //Crosshair
        GUIStyle crosshairStyle = new GUIStyle(GUI.skin.label);
        crosshairStyle.fontSize = 24;
        crosshairStyle.alignment = TextAnchor.MiddleCenter;
        crosshairStyle.normal.textColor = Color.white;

        GUI.Label(
            new Rect(
                Screen.width / 2 - 10,
                Screen.height / 2 - 10,
                20,
                20
            ),
            "+",
            crosshairStyle
        );
        //Crosshair

        if (currentInteractable == null)
            return;

        string message =
            "[E] " + currentInteractable.GetInteractionText();

        GUIStyle style = new GUIStyle(GUI.skin.label);
        style.fontSize = 20;
        style.alignment = TextAnchor.MiddleCenter;
        style.normal.textColor = Color.white;

        GUI.Label(
            new Rect(
                Screen.width / 2 - 150,
                Screen.height / 2 + 35,
                300,
                40
            ),
            message,
            style
        );
    }*/
}
