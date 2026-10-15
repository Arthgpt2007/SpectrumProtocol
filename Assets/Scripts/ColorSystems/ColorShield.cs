
using UnityEngine;
using UnityEngine.Events;

public class ColorShield : MonoBehaviour
{
    [Header("Shield Settings")]
    public CombatColor shieldColor = CombatColor.Blue;
    public bool isDestroyed = false;

    [Header("Visuals")]
    public Renderer shieldRenderer;
    public GameObject shieldVisual;

    [Header("Events")]
    public UnityEvent onShieldDestroyed;

    void Start()
    {
        UpdateVisual();
    }

    public void TakeColorHit(CombatColor attackColor)
    {
        if (isDestroyed) return;

        CombatColor requiredColor =
            CombatColorUtility.Complement(shieldColor);

        if (attackColor == requiredColor)
        {
            DestroyShield();
        }
        else
        {
            Debug.Log(
                "Wrong colour! Shield: " + shieldColor +
                " | Required: " + requiredColor
            );
        }
    }

    void DestroyShield()
    {
        isDestroyed = true;

        Debug.Log("Shield destroyed!");

        onShieldDestroyed?.Invoke();

        if (shieldVisual != null)
            shieldVisual.SetActive(false);
        else if (shieldRenderer != null)
            shieldRenderer.enabled = false;
    }



    void UpdateVisual()
    {
        if (shieldRenderer == null)
            return;

        // Get the selected shield colour
        Color shieldTint =
            CombatColorUtility.ToUnityColor(shieldColor);

        // Get the shield material
        Material mat = shieldRenderer.material;

        // Update the Shader Graph colour property
        if (mat.HasProperty("_BaseColor"))
        {
            mat.SetColor("_BaseColor", shieldTint);
        }
        else
        {
            Debug.LogWarning(
                "Shield material is missing _BaseColor!"
            );
        }
    }


}
