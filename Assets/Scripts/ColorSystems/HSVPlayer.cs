
using UnityEngine;

public class HSVPlayer : MonoBehaviour
{
    [System.Serializable]
    public class CamouflagePreset
    {
        public string presetName;
        [Range(0f, 1f)] public float hue;
        [Range(0f, 1f)] public float saturation = 1f;
        [Range(0f, 1f)] public float value = 1f;
    }

    public CamouflagePreset[] presets =
    {
        new CamouflagePreset {
            presetName = "Dark Blue",
            hue = 0.65f,
            saturation = 0.8f,
            value = 0.3f
        },
        new CamouflagePreset {
            presetName = "Bright Green",
            hue = 0.33f,
            saturation = 0.9f,
            value = 0.9f
        },
        new CamouflagePreset {
            presetName = "Muted Red",
            hue = 0f,
            saturation = 0.5f,
            value = 0.5f
        }
    };

    public int currentPreset;
    public CamouflageZone currentZone;

    public Color CurrentColor
    {
        get
        {
            if (presets == null || presets.Length == 0)
                return Color.white;

            var p = presets[currentPreset];
            return Color.HSVToRGB(p.hue, p.saturation, p.value);
        }
    }

    public bool IsHidden
    {
        get
        {
            return currentZone != null &&
                   currentZone.IsMatching(CurrentColor);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) &&
            presets != null && presets.Length > 0)
        {
            currentPreset =
                (currentPreset + 1) % presets.Length;
        }
    }

    /*void OnGUI()
    {
        if (presets == null || presets.Length == 0)
            return;

        GUI.color = CurrentColor;
        GUI.DrawTexture(
            new Rect(20, Screen.height - 90, 45, 45),
            Texture2D.whiteTexture
        );
        GUI.color = Color.white;

        string status = IsHidden ? "CAMOUFLAGED" : "EXPOSED";

        GUI.Label(
            new Rect(75, Screen.height - 90, 350, 60),
            "Camouflage: " + presets[currentPreset].presetName +
            "\n[C] Change | " + status
        );
    }*/
}
