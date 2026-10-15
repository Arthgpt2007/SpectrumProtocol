
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FPSController : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 5f;
    public float gravity = -20f;

    [Header("Mouse Look")]
    public Transform playerCamera;
    public float mouseSensitivity = 2f;

    private CharacterController controller;
    private float verticalVelocity;
    private float cameraPitch;

    public float jumpHeight = 1.5f;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        if (!LevelIntroManager.BriefingOpen)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    void Update()
    {
        if (Time.timeScale == 0f)
            return;

        // Mouse look
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        cameraPitch -= mouseY;
        cameraPitch = Mathf.Clamp(cameraPitch, -85f, 85f);

        playerCamera.localRotation =
            Quaternion.Euler(cameraPitch, 0f, 0f);

        // Movement
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        Vector3 movement =
            transform.right * x + transform.forward * z;

        movement = Vector3.ClampMagnitude(movement, 1f);

        //Jumping
        if (controller.isGrounded)
        {
            if (verticalVelocity < 0)
                verticalVelocity = -2f;

            if (Input.GetKeyDown(KeyCode.Space))
                verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        verticalVelocity += gravity * Time.deltaTime;

        Vector3 velocity = movement * moveSpeed;
        velocity.y = verticalVelocity;

        controller.Move(velocity * Time.deltaTime);

    }
}
