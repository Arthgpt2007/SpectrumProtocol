
using UnityEngine;

public class RGBSwitch : MonoBehaviour, IInteractable
{
    public RGBPuzzle puzzle;

    [Range(0, 2)]
    public int channel;

    public void Interact()
    {
        if (puzzle != null)
            puzzle.ToggleChannel(channel);
    }

    public string GetInteractionText()
    {
        string[] names = { "Red", "Green", "Blue" };
        return "Toggle " + names[channel] + " Light";
    }
}
