
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    public Transform player;
    public Transform startingCheckpoint;

    private Vector3 respawnPosition;
    private Quaternion respawnRotation;

    private void Awake()
    {
        Instance = this;

        if (startingCheckpoint != null)
        {
            respawnPosition = startingCheckpoint.position;
            respawnRotation = startingCheckpoint.rotation;
        }
        else if (player != null)
        {
            respawnPosition = player.position;
            respawnRotation = player.rotation;
        }
    }

    void Update()
    {
        // Press R to manually respawn (testing only)
        if (Input.GetKeyDown(KeyCode.R))
        {
            Debug.Log("Manual respawn requested!");
            RespawnPlayer();
        }
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        respawnPosition = checkpoint.position;
        respawnRotation = checkpoint.rotation;

        Debug.Log("Checkpoint Activated!");
    }

    public void RespawnPlayer()
    {
        if (player == null) return;

        CharacterController controller =
            player.GetComponent<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        player.SetPositionAndRotation(
            respawnPosition,
            respawnRotation
        );

        if (controller != null)
            controller.enabled = true;

        Debug.Log("Player Respawned!");
    }
}
