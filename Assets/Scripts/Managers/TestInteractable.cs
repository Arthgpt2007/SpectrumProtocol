
using UnityEngine;

public class TestInteractable : MonoBehaviour, IInteractable
{
    private bool activated = false;

    public void Interact()
    {
        activated = !activated;

        GetComponent<Renderer>().material.color =
            activated ? Color.green : Color.red;

        Debug.Log("Button activated: " + activated);
    }

    public string GetInteractionText()
    {
        return activated ? "Deactivate Button" : "Activate Button";
    }
}
