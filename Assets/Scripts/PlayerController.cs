using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance { get; private set; }
    private Vector2 moveInput;
    [SerializeField] private Camera mainCamera;
    private InputAction moveAction;
    public float moveSpeed = 5f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
    }

    // Update is called once per frame
    void Update()
    {
        moveInput.x = moveAction.ReadValue<Vector2>().x;
        if (moveInput.x == -1f && Mouse.current.position.ReadValue().x > (Screen.width - Screen.width / 10)) return;
        if (moveInput.x == 1f && Mouse.current.position.ReadValue().x < Screen.width / 10) return;
        if (moveInput.x == 1f || Mouse.current.position.ReadValue().x > (Screen.width - Screen.width / 10))
        {
            transform.position += moveSpeed * Time.deltaTime * new Vector3(1, 0, 0);
        }
        else if (moveInput.x == -1f || Mouse.current.position.ReadValue().x < Screen.width / 10)
        {
            transform.position += moveSpeed * Time.deltaTime * new Vector3(-1, 0, 0);
        }
    }
}
