using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
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

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private float currentSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

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
        // 입력은 Update 에서 읽습니다. 
        // 키보드 읽기로 임시 구현하였습니다.
        Keyboard keyboard = Keyboard.current;
        if (keyboard == null)
        {
            moveInput = Vector2.zero;
            isMoving = false;
            isRunning = false;
            currentState = "Idle";
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

        // <달리기 처리>
        // 달리기 상태일 시 스피드 변화
        isRunning = shiftHeld && moveInput.sqrMagnitude > 0f;

        if (isRunning)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        // <현재 상태 확인>
        // 현재 움직이는지, 서있는지, 달리는지 상태를 세팅합니다.
        // 강의에서는 다루지 않을 예정이지만, 추후에 애니메이션을 추가할 때 도움이 됩니다.
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
    }

    void FixedUpdate()
    {
        // ----- 이동 처리 -----
        // 키보드 input에 기반하여 이동합니다.
        Vector2 moveDelta = moveInput * currentSpeed * Time.fixedDeltaTime;
        if (moveDelta == Vector2.zero)
        {
            return;
        }

        rb.MovePosition(rb.position + moveDelta);
        distanceTraveled += moveDelta.magnitude;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //임시 게임 오버 기능
        if(collision.collider.CompareTag("Enemy"))
        {
            Debug.Log("당신은 잡혔습니다!");
            Time.timeScale = 0.0f;
        }
    }
    private void OnTriggerEnter2D(Collider2D collider)
    {
        //임시 보물 획득 기능
        if (collider.CompareTag("Goal"))
        {
            Debug.Log("보물을 얻었습니다!");
        }
    }
}
