using UnityEngine;

// 플레이어의 이동, 걷기/달리기 상태 전환, 충돌 처리
//
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerInputSystem))]
public class PlayerMovement : MonoBehaviour
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
    private PlayerInputSystem input;
    private Vector2 moveInput;
    private float currentSpeed;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        // --------------------- 여기에 넣을 것을 작성하세요. ---------------------------//
        input = GetComponent<PlayerInputSystem>();
        // ------------------------------------------------------------------------------//
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
        // ----- 입력 반영 -----
        // 입력은 PlayerInputSystem에서 읽어오고, 여기서는 그 결과를 사용합니다.
        // --------------------- 여기에 넣을 것을 작성하세요. ---------------------------//
        moveInput = input.moveInput;
        // ------------------------------------------------------------------------------//

        // <현재 상태 확인>
        // 현재 움직이는지, 서있는지, 달리는지 상태를 세팅합니다.
        // 강의에서는 다루지 않을 예정이지만, 추후에 애니메이션을 추가할 때 도움이 됩니다.
        isMoving = moveInput.sqrMagnitude > 0f;

        // <달리기 처리>
        // 달리기 상태일 시 스피드 변화합니다.
        // --------------------- 여기에 넣을 것을 작성하세요. ---------------------------//
        isRunning = input.runHeld && isMoving;
        // ------------------------------------------------------------------------------//


        if (isRunning)
        {
            currentSpeed = runSpeed;
        }
        else
        {
            currentSpeed = moveSpeed;
        }

        

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
        // 입력에 기반하여 이동합니다.
        Vector2 moveDelta = moveInput * currentSpeed * Time.fixedDeltaTime; 
        if (moveDelta == Vector2.zero)
        {
            return;
        }

        rb.MovePosition(rb.position + moveDelta);
        distanceTraveled += moveDelta.magnitude;
    }

    // 아래 두 충돌 판정은 이동 로직과 직접적인 관련은 없는 임시 코드다.
    // 추후에 정식 상호작용 시스템으로 대체될 예정이라 지금은 여기 임시로 둔다.
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
