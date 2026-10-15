
using UnityEngine;
using UnityEngine.AI;

public class DroneHealth : MonoBehaviour
{
    [Header("Drone Health")]
    public int maxHealth = 1;

    [Header("Optional Shield")]
    public ColorShield shield;

    [Header("References")]
    public SecurityDrone droneAI;
    public NavMeshAgent agent;

    [Header("Destruction")]
    public GameObject destructionEffect;
    public bool destroyGameObject = true;

    private int currentHealth;
    private bool isDead;

    void Start()
    {
        currentHealth = Mathf.Max(1, maxHealth);
    }

    public void TakeDamage(int damage)
    {
        if (isDead) return;

        // Drone cannot be damaged while shield is active
        if (shield != null && !shield.isDestroyed)
        {
            Debug.Log("Drone protected by shield!");
            return;
        }

        currentHealth -= damage;

        Debug.Log(
            gameObject.name + " HP: " + currentHealth
        );

        if (currentHealth <= 0)
            Die();
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;

        if (droneAI != null)
            droneAI.enabled = false;

        if (agent != null && agent.enabled)
        {
            if (agent.isOnNavMesh)
                agent.isStopped = true;

            agent.enabled = false;
        }

        if (destructionEffect != null)
        {
            Instantiate(
                destructionEffect,
                transform.position,
                Quaternion.identity
            );
        }

        Debug.Log(gameObject.name + " DESTROYED!");

        if (destroyGameObject)
            Destroy(gameObject);
        else
            gameObject.SetActive(false);
    }
}
