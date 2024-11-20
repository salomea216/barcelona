using UnityEngine;
using UnityEngine.UI; // For UI elements like black bars

public class CameraController : MonoBehaviour
{
    public Transform player;              // The player's transform
    public float smoothSpeed = 5f;        // Speed of camera smoothing
    public Vector3 offset;                // Offset for camera's position
    public float centerSmoothingFactor = 2f; // Smoothing factor for centering the camera
    public GameObject blackBarsPrefab;    // Prefab for black bars (UI overlay)

    private GameObject blackBars;         // Reference to instantiated black bars
    private bool isCinematicMode = false; // Toggles cinematic mode on/off
    private bool isCameraCentered = false; // Flag to check if the camera is centered behind the player

    private float timer = 0f;             // Timer to track when to start centering
    private float delayTime = 1f;         // Time to wait before centering after stopping (1 second)
    private bool isFacingCamera = false;  // Track if the player is facing the camera
    private float lastFacingCameraTime = 0f; // Time when the player last started facing the camera

    void Start()
    {
        // Instantiate black bars but keep them hidden initially
        if (blackBarsPrefab != null)
        {
            blackBars = Instantiate(blackBarsPrefab);
            blackBars.SetActive(false); // Hide black bars at the start
        }
    }

    void Update()
    {
        HandleCinematicInput(); // Handle Q input to toggle cinematic mode

        // Check if the player is facing the camera (looking toward the negative Z-axis)
        CheckPlayerFacingCamera();

        // Check for player movement and start timer when stationary
        if (Input.GetAxis("Horizontal") == 0 && Input.GetAxis("Vertical") == 0)
        {
            // Player is not moving, start the timer
            timer += Time.deltaTime;
        }
        else
        {
            // Player is moving, reset the timer
            timer = 0f;
        }

        // Adjust movement based on camera being centered
        if (isCameraCentered)
        {
            HandleReversedPlayerMovement(); // Handle reversed movement when camera is centered behind player
        }
        else
        {
            HandleNormalPlayerMovement(); // Regular player movement controls
        }
    }

    void LateUpdate()
    {
        if (isCinematicMode)
        {
            HandleCinematicCameraBehindPlayer(); // Focus camera behind player with black bars
        }
        else
        {
            if (timer >= delayTime)
            {
                // Once timer reaches 1 second, center the camera behind the player
                HandleCameraFocusBehindPlayer();
            }
            else
            {
                HandleNormalCameraBehavior(); // Default camera behavior
            }
        }

        // Adjust camera position based on the player's direction (if they're facing the camera)
        if (isFacingCamera && Time.time - lastFacingCameraTime >= 1f)
        {
            HandleCameraRotationToBack();
        }
    }

    void HandleCinematicInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            isCinematicMode = !isCinematicMode; // Toggle cinematic mode

            if (blackBars != null)
            {
                blackBars.SetActive(isCinematicMode); // Show or hide black bars based on mode
            }

            // Toggle camera centered state when Q is pressed
            isCameraCentered = !isCameraCentered;
        }
    }

    void HandleCinematicCameraBehindPlayer()
    {
        // Align the camera directly behind the player in cinematic mode
        Vector3 targetPosition = player.position - player.forward * Mathf.Abs(offset.z);
        targetPosition.y = player.position.y + offset.y; // Maintain height

        // Smoothly transition to the cinematic position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * centerSmoothingFactor);

        // Keep the camera looking at the player
        transform.LookAt(player.position + Vector3.up * offset.y);
    }

    void HandleNormalCameraBehavior()
    {
        // Default camera behavior: camera smoothly follows the player with the offset
        Vector3 desiredPosition = player.position + offset;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.LookAt(player.position + Vector3.up * offset.y);
    }

    void HandleCameraFocusBehindPlayer()
    {
        // When Q is pressed, the camera behaves like the main camera, without rotation
        Vector3 targetPosition = player.position - player.forward * Mathf.Abs(offset.z);
        targetPosition.y = player.position.y + offset.y; // Maintain height

        // Smoothly transition to the fixed position behind the player
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);

        // Keep the camera looking at the player
        transform.LookAt(player.position + Vector3.up * offset.y);
    }

    void HandleCameraRotationToBack()
    {
        // Smoothly rotate the camera to the back of the player
        Vector3 targetPosition = player.position - player.forward * Mathf.Abs(offset.z); // Camera behind the player
        targetPosition.y = player.position.y + offset.y; // Maintain the height

        // Smoothly transition the camera to the target position
        transform.position = Vector3.Lerp(transform.position, targetPosition, Time.deltaTime * smoothSpeed);

        // Keep the camera looking at the player
        transform.LookAt(player.position + Vector3.up * offset.y);
    }

    void CheckPlayerFacingCamera()
    {
        // Check if the player is facing the camera (looking towards the negative Z-axis)
        Vector3 playerToCamera = transform.position - player.position; // Vector from player to camera
        float dotProduct = Vector3.Dot(player.forward, playerToCamera.normalized); // Dot product of player’s forward direction and the vector to the camera

        // If the dot product is less than 0, the player is facing the camera (more than 90 degrees)
        if (dotProduct < 0)
        {
            isFacingCamera = true;
            lastFacingCameraTime = Time.time;
        }
        else
        {
            isFacingCamera = false;
        }
    }

    void HandleReversedPlayerMovement()
    {
        // Handle reversed movement when the camera is centered behind the player
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Reverse the forward/backward movement when the camera is centered
        if (verticalInput != 0)
        {
            verticalInput = -verticalInput; // Invert vertical input (W and S controls are reversed)
        }

        // Move the player using the camera's local direction (transform.forward and transform.right)
        Vector3 forwardMovement = transform.forward * verticalInput; // Forward/backward movement
        Vector3 rightMovement = transform.right * horizontalInput;  // Left/right movement

        // Combine the movements and normalize them to avoid faster diagonal movement
        Vector3 movement = (forwardMovement + rightMovement).normalized;

        // Move the player using the calculated movement vector
        player.Translate(movement * Time.deltaTime * 5f, Space.World); // Adjust movement speed as needed
    }

    void HandleNormalPlayerMovement()
    {
        // Regular movement control for the player (no reversal of controls)
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Use the camera's local directions (forward and right) for movement alignment
        Vector3 forwardMovement = transform.forward * verticalInput; // Forward/backward movement
        Vector3 rightMovement = transform.right * horizontalInput;  // Left/right movement

        // Combine movements and normalize to avoid faster diagonal movement
        Vector3 movement = (forwardMovement + rightMovement).normalized;

        // Move the player using the calculated movement vector
        player.Translate(movement * Time.deltaTime * 5f, Space.World); // Adjust movement speed as needed
    }
}
