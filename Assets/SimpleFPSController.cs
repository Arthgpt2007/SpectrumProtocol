using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SimpleFPSController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private float gravity = -9.81f;

    [Header("Mouse Look")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float mouseSensitivity = 2.0f;
    [SerializeField] private float maxLookAngle = 80.0f;

    private CharacterController controller;
    private float verticalVelocity = 0.0f;
    private float cameraPitch = 0.0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // 1. Mouse Look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -maxLookAngle, maxLookAngle);
        if (cameraTransform != null)
        {
            cameraTransform.localEulerAngles = Vector3.right * cameraPitch;
        }

        // 2. WASD Movement
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        Vector3 moveDirection = (transform.right * moveX + transform.forward * moveZ).normalized;

        // 3. Gravity & Ground Check
        if (controller.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2.0f; // Keeps the player glued to ramps
        }
        else
        {
            verticalVelocity += gravity * Time.deltaTime;
        }

        Vector3 totalVelocity = (moveDirection * moveSpeed) + (Vector3.up * verticalVelocity);
        controller.Move(totalVelocity * Time.deltaTime);

        // 4. Escape key to free cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}