
using UnityEngine;
using UnityEngine.EventSystems;

public class ColorGun : MonoBehaviour
{
    [Header("References")]
    public Camera playerCamera;

    [Header("Eye Laser")]
    public Transform eyeMuzzle;

    [Header("Gun Settings")]
    public float range = 30f;
    public float fireCooldown = 0.25f;
    public LayerMask hitLayers = ~0;

    [Header("Visuals")]
    public LineRenderer beam;

    public static float blockShootingUntil;


    public CombatColor selectedColor = CombatColor.Red;

    private float nextFireTime;
    private float beamHideTime;

    void Update()
    {

        if (Time.unscaledTime < blockShootingUntil)
            return;

        if (LevelIntroManager.BriefingOpen)
            return;

        if (Time.timeScale == 0f)
            return;


        // Select colour using number keys
        for (int i = 0; i < 6; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                selectedColor = (CombatColor)i;
                Debug.Log("Selected: " + selectedColor);
            }
        }

        // Fire

        if (Input.GetMouseButtonDown(0) &&
            Time.time >= nextFireTime &&
            Cursor.lockState == CursorLockMode.Locked)
        {
            bool clickingUI =
                EventSystem.current != null &&
                EventSystem.current.IsPointerOverGameObject();

            if (!clickingUI)
            {
                Fire();
                nextFireTime = Time.time + fireCooldown;
            }
        }


        // Hide beam after brief flash
        if (beam != null &&
            beam.enabled &&
            Time.time >= beamHideTime)
        {
            beam.enabled = false;
        }
    }

    void Fire()
    {
        if (playerCamera == null) return;

        Vector3 origin = playerCamera.transform.position;
        Vector3 direction = playerCamera.transform.forward;
        Vector3 endPoint = origin + direction * range;

        if (Physics.Raycast(
            origin,
            direction,
            out RaycastHit hit,
            range,
            hitLayers,
            QueryTriggerInteraction.Collide))
        {
            endPoint = hit.point;

            // First check whether the shot hit an active shield
            ColorShield shield =
                hit.collider.GetComponentInParent<ColorShield>();

            if (shield != null && !shield.isDestroyed)
            {
                shield.TakeColorHit(selectedColor);
            }
            else
            {
                // Otherwise, damage an exposed drone
                DroneHealth drone =
                    hit.collider.GetComponentInParent<DroneHealth>();

                if (drone != null)
                    drone.TakeDamage(1);
            }
        }

        if (beam != null)
        {
            beam.positionCount = 2;
            beam.useWorldSpace = true;

            Vector3 beamStart = eyeMuzzle != null
     ? eyeMuzzle.position
     : playerCamera.transform.position
       + playerCamera.transform.forward * 0.5f;

            beam.SetPosition(0, beamStart);
            beam.SetPosition(1, endPoint);

            Color shotColor =
                CombatColorUtility.ToUnityColor(selectedColor);

            beam.startColor = shotColor;
            beam.endColor = shotColor;
            beam.startWidth = 0.12f;
            beam.endWidth = 0.035f;

            beam.enabled = true;
            beamHideTime = Time.time + 0.18f;
        }
    }

    /*void OnGUI()
    {
        string message =
            "COLOR GUN: " + selectedColor +
            "\n1 Red | 2 Cyan | 3 Blue | 4 Orange" +
            "\n5 Green | 6 Magenta";

        GUI.Label(
            new Rect(
                Screen.width - 300,
                Screen.height - 90,
                290,
                80
            ),
            message
        );
    }*/
}
