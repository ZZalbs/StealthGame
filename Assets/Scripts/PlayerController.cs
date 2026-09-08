using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("이동 설정")]
    public float moveSpeed = 3f;
    public float runSpeed = 6f;

    [Header("상태")]
    public bool isMoving;
    public bool isRunning;
    public string currentState = "Idle";
    public Vector2 lastMoveDirection;
    public float distanceTraveled;

    private Vector2 moveInput;
    private float currentSpeed;

    void Start()
    {
        moveInput = Vector2.zero;
        currentSpeed = moveSpeed;
        currentState = "Idle";
        distanceTraveled = 0f;
    }

    void Update()
    {
        // ----- 입력 처리 -----
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        float h = 0f;
        float v = 0f;

        if (keyboard.aKey.isPressed)
        {
            h -= 1f;
        }
        if (keyboard.dKey.isPressed)
        {
            h += 1f;
        }
        if (keyboard.sKey.isPressed)
        {
            v -= 1f;
        }
        if (keyboard.wKey.isPressed)
        {
            v += 1f;
        }

        moveInput = new Vector2(h, v);
        if (moveInput.sqrMagnitude > 1f)
        {
            moveInput.Normalize();
        }

        bool shiftHeld = keyboard.leftShiftKey.isPressed || keyboard.rightShiftKey.isPressed;

        // ----- 달리기 처리 -----
        isRunning = shiftHeld && moveInput.sqrMagnitude > 0f;

        if (isRunning)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        // ----- 이동 처리 -----
        Vector3 moveDelta = new Vector3(moveInput.x, moveInput.y, 0f) * currentSpeed * Time.deltaTime;
        transform.position += moveDelta;

        // ----- 상태 변수 관리 -----
        isMoving = moveInput.sqrMagnitude > 0f;

        if (isMoving)
        {
            lastMoveDirection = moveInput;
        }

        if (isRunning)
        {
            currentState = "Run";
        }
        else if (isMoving)
        {
            currentState = "Walk";
        }
        else
        {
            currentState = "Idle";
        }

        distanceTraveled += moveDelta.magnitude;
    }
}
