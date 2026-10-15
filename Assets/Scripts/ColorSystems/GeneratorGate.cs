
using UnityEngine;

public class GeneratorGate : MonoBehaviour
{
    public ShieldGenerator[] generators;
    public GameObject securityGate;

    private bool unlocked;

    void Update()
    {
        if (unlocked) return;

        if (generators == null || generators.Length == 0)
            return;

        foreach (ShieldGenerator generator in generators)
        {
            if (generator == null || !generator.isDisabled)
                return;
        }

        unlocked = true;

        if (securityGate != null)
            securityGate.SetActive(false);

        Debug.Log("All generators disabled. Gate unlocked!");
    }
}
