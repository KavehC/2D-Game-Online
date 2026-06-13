using UnityEngine;
using Mirror;
using UnityEngine.InputSystem;

public class Player_Behavior : NetworkBehaviour
{
    [SerializeField] float walkSpeed = 1f;
    [SerializeField] float runSpeed = 2f;

    private float currentSpeed;
    private bool isRunning;

    Rigidbody2D rb;
    Vector2 input;
    SmoothCharacterTracking cam;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public override void OnStartLocalPlayer()
    {
        cam = FindObjectOfType<SmoothCharacterTracking>();

        if (cam == null && Camera.main != null)
            cam = Camera.main.GetComponent<SmoothCharacterTracking>();

        if (cam != null)
            cam.SetTarget(transform);
    }

    void Update()
    {
        if (!isLocalPlayer) return;

        input = ReadInput();

        isRunning = Keyboard.current.leftShiftKey.isPressed;
        currentSpeed = isRunning ? runSpeed : walkSpeed;
    }

    void FixedUpdate()
    {
        if (!isLocalPlayer) return;

        rb.MovePosition(
            rb.position + input * currentSpeed * Time.fixedDeltaTime
        );
    }

    Vector2 ReadInput()
    {
        Vector2 result = Vector2.zero;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) result.x -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) result.x += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) result.y -= 1f;
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) result.y += 1f;
        }

        if (Gamepad.current != null)
            result += Gamepad.current.leftStick.ReadValue();

        return Vector2.ClampMagnitude(result, 1f);
    }

    // Animator access
    public Vector2 Movement => input;
    public bool IsRunning => isRunning;
    public float CurrentSpeed => currentSpeed;
}