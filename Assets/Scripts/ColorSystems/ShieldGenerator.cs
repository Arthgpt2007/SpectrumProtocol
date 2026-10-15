
using UnityEngine;
using UnityEngine.Events;

public class ShieldGenerator : MonoBehaviour
{
    public ColorShield shield;

    public Renderer generatorRenderer;

    public bool isDisabled;

    public UnityEvent onGeneratorDisabled;

    void Start()
    {
        if (shield != null)
            shield.onShieldDestroyed.AddListener(DisableGenerator);
    }

    public void DisableGenerator()
    {
        if (isDisabled) return;

        isDisabled = true;

        if (generatorRenderer != null)
            generatorRenderer.material.color = Color.gray;

        onGeneratorDisabled?.Invoke();

        Debug.Log("Generator disabled!");
    }

    void OnDestroy()
    {
        if (shield != null)
            shield.onShieldDestroyed.RemoveListener(DisableGenerator);
    }
}
