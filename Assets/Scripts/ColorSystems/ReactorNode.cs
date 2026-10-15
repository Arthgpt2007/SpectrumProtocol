
using UnityEngine;

public class ReactorNode : MonoBehaviour, IInteractable
{
    [Header("Reactor Settings")]
    public ReactorController reactor;

    [Range(0, 5)]
    public int colorIndex;

    [Header("Visual Settings")]
    public Renderer nodeRenderer;

    [Range(0f, 1f)]
    public float inactiveBrightness = 0.45f;

    [Range(0f, 1f)]
    public float activeBrightness = 1f;

    [Header("Debug")]
    public bool showDebugLogs = false;

    private bool selected;
    private Material nodeMaterial;

    // Distinct colours in circular order
    private readonly float[] hues =
    {
        0f,        // Red
        0.08333f,  // Orange
        0.16667f,  // Yellow
        0.33333f,  // Green
        0.66667f,  // Blue
        0.77778f   // Violet
    };

    private readonly string[] colorNames =
    {
        "Red",
        "Orange",
        "Yellow",
        "Green",
        "Blue",
        "Violet"
    };

    void Awake()
    {
        if (nodeRenderer == null)
            nodeRenderer = GetComponent<Renderer>();

        if (nodeRenderer != null)
        {
            // Create a separate material instance
            nodeMaterial = nodeRenderer.material;
        }
        else
        {
            Debug.LogError(
                "No Renderer assigned on " + gameObject.name
            );
        }
    }

    void Start()
    {
        SetSelected(false);
    }

    public void Interact()
    {
        if (reactor != null)
            reactor.SelectColor(colorIndex);
    }

    public string GetInteractionText()
    {
        return selected
            ? "Deselect " + colorNames[colorIndex]
            : "Select " + colorNames[colorIndex];
    }

    public void SetSelected(bool value)
    {
        selected = value;

        if (nodeMaterial == null)
            return;

        int index = Mathf.Clamp(colorIndex, 0, 5);

        float hue = hues[index];

        float brightness = selected
            ? activeBrightness
            : inactiveBrightness;

        // Keep hue and saturation constant.
        // Change only HSV brightness.
        Color finalColor = Color.HSVToRGB(
            hue,
            1f,
            brightness
        );

        // URP Lit and URP Unlit
        if (nodeMaterial.HasProperty("_BaseColor"))
        {
            nodeMaterial.SetColor(
                "_BaseColor",
                finalColor
            );
        }
        // Built-in Standard shader fallback
        else if (nodeMaterial.HasProperty("_Color"))
        {
            nodeMaterial.SetColor(
                "_Color",
                finalColor
            );
        }

        // Prevent emission from overriding base colour
        if (nodeMaterial.HasProperty("_EmissionColor"))
        {
            nodeMaterial.SetColor(
                "_EmissionColor",
                Color.black
            );
        }

        if (showDebugLogs)
        {
            Debug.Log(
                gameObject.name +
                " | Index: " + index +
                " | Colour: " + colorNames[index] +
                " | Selected: " + selected +
                " | RGB: " + finalColor
            );
        }
    }

    void OnDestroy()
    {
        if (nodeMaterial != null)
            Destroy(nodeMaterial);
    }
}
