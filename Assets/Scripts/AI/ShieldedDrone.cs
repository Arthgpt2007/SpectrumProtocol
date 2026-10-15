
using UnityEngine;
using UnityEngine.AI;

public class ShieldedDrone : MonoBehaviour
{
    public ColorShield shield;
    public SecurityDrone droneAI;
    public NavMeshAgent agent;

    private void Start()
    {
        if (shield != null)
            shield.onShieldDestroyed.AddListener(DisableDrone);
    }

    public void DisableDrone()
    {
        if (droneAI != null)
            droneAI.enabled = false;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        Debug.Log("Security drone disabled!");
    }

    private void OnDestroy()
    {
        if (shield != null)
            shield.onShieldDestroyed.RemoveListener(DisableDrone);
    }
}
