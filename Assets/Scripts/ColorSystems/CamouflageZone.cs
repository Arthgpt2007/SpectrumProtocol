
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CamouflageZone : MonoBehaviour
{
    public Color targetColor = Color.blue;
    [Range(0f, 1.732f)]
    public float tolerance = 0.15f;

    public Renderer zoneRenderer;

    void Start()
    {
        GetComponent<Collider>().isTrigger = true;

        if (zoneRenderer != null)
            zoneRenderer.material.color = targetColor;
    }

    public bool IsMatching(Color playerColor)
    {
        Vector3 difference = new Vector3(
            playerColor.r - targetColor.r,
            playerColor.g - targetColor.g,
            playerColor.b - targetColor.b
        );

        return difference.magnitude <= tolerance;
    }

    void OnTriggerEnter(Collider other)
    {
        HSVPlayer player = other.GetComponent<HSVPlayer>();

        if (player != null)
            player.currentZone = this;
    }

    void OnTriggerExit(Collider other)
    {
        HSVPlayer player = other.GetComponent<HSVPlayer>();

        if (player != null && player.currentZone == this)
            player.currentZone = null;
    }
}
