
using UnityEngine;

public static class CombatColorUtility
{
    public static CombatColor Complement(CombatColor color)
    {
        switch (color)
        {
            case CombatColor.Red: return CombatColor.Cyan;
            case CombatColor.Cyan: return CombatColor.Red;
            case CombatColor.Blue: return CombatColor.Orange;
            case CombatColor.Orange: return CombatColor.Blue;
            case CombatColor.Green: return CombatColor.Magenta;
            case CombatColor.Magenta: return CombatColor.Green;
            default: return CombatColor.Red;
        }
    }

    public static Color ToUnityColor(CombatColor color)
    {
        switch (color)
        {
            case CombatColor.Red: return Color.red;
            case CombatColor.Cyan: return Color.cyan;
            case CombatColor.Blue: return Color.blue;
            case CombatColor.Orange:
                return new Color(1f, 0.5f, 0f);
            case CombatColor.Green: return Color.green;
            case CombatColor.Magenta: return Color.magenta;
            default: return Color.white;
        }
    }
}
