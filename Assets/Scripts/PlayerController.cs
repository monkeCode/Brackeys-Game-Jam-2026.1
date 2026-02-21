using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    
    [Header("Movement Settings")]
    [SerializeField] private float baseSpeed = 5f;
    [SerializeField] private float maxSpeedMultiplier = 3f;
    [SerializeField] private AnimationCurve speedCurve = AnimationCurve.EaseInOut(0f, 1f, 1f, 3f);
    
    [Header("Edge Detection")]
    [SerializeField] [Range(0.05f, 0.3f)] private float edgeZoneSize = 0.1f; // 10% of screen width
    
    private Vector2 moveInput;
    [SerializeField] private Camera mainCamera;
    private InputAction moveAction;
    private float currentSpeed;
    
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        
        moveAction = InputSystem.actions.FindAction("Move");
        
        // Cache the main camera if not assigned
        if (mainCamera == null)
            mainCamera = Camera.main;
    }
    
    void Update()
    {
        moveInput.x = moveAction.ReadValue<Vector2>().x;
        HandleMovement();
    }
    
    private void HandleMovement()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        float direction = 0f;
        
        // Calculate speed multiplier and direction based on input
        float speedMultiplier = 1f;
        
        // Keyboard input takes priority
        if (Mathf.Abs(moveInput.x) > 0.1f)
        {
            direction = Mathf.Sign(moveInput.x);
            speedMultiplier = maxSpeedMultiplier;
        }
        // Mouse edge detection
        else
        {
            float leftEdge = Screen.width * edgeZoneSize;
            float rightEdge = Screen.width * (1f - edgeZoneSize);
            
            if (mousePos.x < leftEdge)
            {
                direction = -1f;
                // Calculate speed based on distance from edge
                float distanceFromEdge = Mathf.Clamp01(1f - (mousePos.x / leftEdge));
                speedMultiplier = speedCurve.Evaluate(distanceFromEdge);
            }
            else if (mousePos.x > rightEdge)
            {
                direction = 1f;
                // Calculate speed based on distance from edge
                float distanceFromEdge = Mathf.Clamp01((mousePos.x - rightEdge) / (Screen.width * edgeZoneSize));
                speedMultiplier = speedCurve.Evaluate(distanceFromEdge);
            }
        }
        
        // Apply movement if we have a direction
        if (direction != 0f)
        {
            currentSpeed = baseSpeed * speedMultiplier;
            Vector3 movement = direction * currentSpeed * Time.deltaTime * Vector3.right;
            transform.position += movement;
        }
    }
    
    // Optional: Visual feedback in the inspector
    private void OnGUI()
    {
        if (!Application.isPlaying) return;
        
        // Display current speed in top-left corner (for debugging)
        GUI.Label(new Rect(10, 10, 200, 20), $"Speed: {currentSpeed:F1}");
        GUI.Label(new Rect(10, 30, 200, 20), $"Input: {moveInput.x:F1}");
    }
    
    // Visualize edge zones in Scene view
    private void OnDrawGizmosSelected()
    {
        if (mainCamera == null) return;
        
        Vector3 topLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, Screen.height, 0));
        Vector3 topRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        Vector3 bottomLeft = mainCamera.ScreenToWorldPoint(Vector3.zero);
        Vector3 bottomRight = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, 0, 0));
        
        float edgeWorldWidth = (topRight.x - topLeft.x) * edgeZoneSize;
        
        // Left edge zone
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f); // Semi-transparent yellow
        Vector3 leftZoneMin = new Vector3(topLeft.x, bottomLeft.y, 0);
        Vector3 leftZoneMax = new Vector3(topLeft.x + edgeWorldWidth, topLeft.y, 0);
        Gizmos.DrawCube((leftZoneMin + leftZoneMax) * 0.5f, leftZoneMax - leftZoneMin);
        
        // Right edge zone
        Gizmos.color = new Color(1f, 1f, 0f, 0.3f);
        Vector3 rightZoneMin = new Vector3(topRight.x - edgeWorldWidth, bottomRight.y, 0);
        Vector3 rightZoneMax = new Vector3(topRight.x, topRight.y, 0);
        Gizmos.DrawCube((rightZoneMin + rightZoneMax) * 0.5f, rightZoneMax - rightZoneMin);
    }
}