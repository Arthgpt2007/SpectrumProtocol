
using UnityEngine;

public class RGBPuzzle : MonoBehaviour
{
    [Header("Current Channels")]
    public bool red;
    public bool green;
    public bool blue;

    [Header("Required Combination")]
    public bool requiredRed = true;
    public bool requiredGreen = true;
    public bool requiredBlue = false;

    [Header("References")]
    public Renderer outputRenderer;
    public GameObject securityGate;

    private bool unlocked;

    void Start()
    {
        // Default output colour is white
        if (outputRenderer != null)
            outputRenderer.material.color = Color.white;
    }

    public void ToggleChannel(int channel)
    {
        if (unlocked) return;

        switch (channel)
        {
            case 0: red = !red; break;
            case 1: green = !green; break;
            case 2: blue = !blue; break;
        }

        UpdatePuzzle();
    }

    void UpdatePuzzle()
    {
        Color mixedColor = new Color(
            red ? 1f : 0f,
            green ? 1f : 0f,
            blue ? 1f : 0f
        );

        if (outputRenderer != null)
            outputRenderer.material.color = mixedColor;

        bool correct =
            red == requiredRed &&
            green == requiredGreen &&
            blue == requiredBlue;

        if (correct && !unlocked)
        {
            unlocked = true;

            if (securityGate != null)
                securityGate.SetActive(false);

            Debug.Log("RGB Gate Unlocked!");
        }
    }
}
