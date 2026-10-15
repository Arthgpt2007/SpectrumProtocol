
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SecurityDrone : MonoBehaviour
{
    public enum DroneState
    {
        Patrol,
        Chase,
        Search
    }

    [Header("References")]
    public Transform player;
    public Transform eyePoint;
    public Transform[] patrolPoints;
    public Renderer droneRenderer;

    [Header("Movement")]
    public float patrolSpeed = 2.5f;
    public float chaseSpeed = 5f;
    public float waypointDistance = 0.6f;
    public float catchDistance = 1.4f;

    [Header("Vision")]
    public float visionRange = 12f;
    [Range(0f, 180f)]
    public float visionAngle = 90f;
    public LayerMask obstacleMask;

    [Header("Search")]
    public float searchDuration = 3f;

    public DroneState currentState = DroneState.Patrol;

    private NavMeshAgent agent;
    private HSVPlayer hsvPlayer;

    private int patrolIndex;
    private Vector3 lastKnownPosition;
    private float searchTimer;
    private float nextPathUpdate;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        if (player != null)
            hsvPlayer = player.GetComponent<HSVPlayer>();

        agent.speed = patrolSpeed;

        GoToNextPatrolPoint();
    }

    private void Update()
    {
        if (player == null || !agent.isOnNavMesh)
            return;

        bool canSeePlayer = CanSeePlayer();

        if (canSeePlayer)
        {
            lastKnownPosition = player.position;

            if (currentState != DroneState.Chase)
                EnterChase();
        }
        else if (currentState == DroneState.Chase)
        {
            EnterSearch();
        }

        switch (currentState)
        {
            case DroneState.Patrol:
                UpdatePatrol();
                break;

            case DroneState.Chase:
                UpdateChase();
                break;

            case DroneState.Search:
                UpdateSearch();
                break;
        }

        UpdateVisuals();
    }


    private bool CanSeePlayer()
    {
        if (player == null)
        {
            Debug.LogWarning("Drone: Player reference missing!");
            return false;
        }

        if (hsvPlayer != null && hsvPlayer.IsHidden)
        {
            Debug.Log("Drone: Player is camouflaged.");
            return false;
        }

        Vector3 origin = eyePoint != null
            ? eyePoint.position
            : transform.position + Vector3.up;

        Vector3 target = player.position + Vector3.up;

        Vector3 direction = target - origin;

        // Draw vision ray in Scene view
        Debug.DrawLine(origin, target, Color.magenta);

        float distance = direction.magnitude;

        if (distance > visionRange)
            return false;

        // Calculate horizontal direction to player
        Vector3 flatDirection = direction;
        flatDirection.y = 0f;

        // Calculate horizontal direction drone is facing
        Vector3 flatForward = transform.forward;
        flatForward.y = 0f;

        float angle = Vector3.Angle(
            flatForward.normalized,
            flatDirection.normalized
        );

        if (angle > visionAngle * 0.5f)
            return false;

        if (Physics.Linecast(
            origin,
            target,
            obstacleMask,
            QueryTriggerInteraction.Ignore))
        {
            Debug.DrawLine(origin, target, Color.yellow);
            return false;
        }

        Debug.DrawLine(origin, target, Color.green);

        return true;
    }


    private void EnterChase()
    {
        currentState = DroneState.Chase;
        agent.speed = chaseSpeed;
        agent.isStopped = false;

        Debug.Log("Drone detected player!");
    }

    private void EnterSearch()
    {
        currentState = DroneState.Search;
        agent.speed = patrolSpeed;
        searchTimer = searchDuration;
        agent.SetDestination(lastKnownPosition);
    }

    private void UpdatePatrol()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        if (!agent.pathPending &&
            agent.remainingDistance <= waypointDistance)
        {
            GoToNextPatrolPoint();
        }
    }

    private void GoToNextPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        if (!agent.isOnNavMesh)
            return;

        agent.SetDestination(
            patrolPoints[patrolIndex].position
        );

        patrolIndex =
            (patrolIndex + 1) % patrolPoints.Length;
    }

    private void UpdateChase()
    {
        if (Time.time >= nextPathUpdate)
        {
            agent.SetDestination(player.position);
            nextPathUpdate = Time.time + 0.15f;
        }

        if (Vector3.Distance(
            transform.position,
            player.position
        ) <= catchDistance)
        {
            CatchPlayer();
        }
    }

    private void UpdateSearch()
    {
        searchTimer -= Time.deltaTime;

        if (searchTimer <= 0f)
        {
            currentState = DroneState.Patrol;
            agent.speed = patrolSpeed;
            GoToNextPatrolPoint();
        }
    }

    private void CatchPlayer()
    {
        Debug.Log("DRONE CAUGHT PLAYER!");

        if (CheckpointManager.Instance != null)
        {
            CheckpointManager.Instance.RespawnPlayer();
        }
        else
        {
            Debug.LogError("CheckpointManager not found!");
        }

        currentState = DroneState.Patrol;
        agent.speed = patrolSpeed;
        GoToNextPatrolPoint();
    }

    private void UpdateVisuals()
    {
        if (droneRenderer == null)
            return;

        Color stateColor = Color.cyan;

        if (currentState == DroneState.Chase)
            stateColor = Color.red;
        else if (currentState == DroneState.Search)
            stateColor = Color.yellow;

        droneRenderer.material.color = stateColor;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(
            transform.position + Vector3.up,
            transform.forward * visionRange
        );

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(
            transform.position,
            visionRange
        );

        if (patrolPoints == null)
            return;

        Gizmos.color = Color.cyan;

        foreach (Transform point in patrolPoints)
        {
            if (point != null)
                Gizmos.DrawSphere(point.position, 0.25f);
        }
    }
}
