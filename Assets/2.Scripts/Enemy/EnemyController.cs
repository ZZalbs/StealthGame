using System.Collections.Generic;
using UnityEngine;

// [2차시 State Pattern - After 코드]
// Before 코드는 Update() 안에 순찰/감지/추적/상태전환이 전부 if/else로
// 뒤섞여 있어, 상태가 하나 늘어날 때마다 그 하나의 메서드가 계속 길어지고
// 서로 다른 상태의 로직이 한 곳에서 얽혀 수정하기 위험했다.
//
// State Pattern으로 "순찰 중에 할 일"과 "추적 중에 할 일"을 PatrolState /
// ChaseState 클래스로 각각 분리했다. EnemyController는 더 이상 "지금 뭘
// 해야 하는지"를 판단하지 않고, 새로운 상태(예: 놀람, 복귀)가 추가되어도
// EnemyController나 기존 상태 클래스는 건드릴 필요가 없다.

[RequireComponent(typeof(Rigidbody2D))]
public class EnemyController : MonoBehaviour
{
    [Header("순찰 설정")]
    public List<Transform> patrolPoints;
    public float patrolSpeed = 2f;

    [Header("추적 설정")]
    public Transform player;
    public float chaseSpeed = 3.5f;
    public float detectionRange = 4f;

    [Header("감지 범위 시각화")]
    public GameObject detectionRangeVisual;

    [Header("상태 (읽기 전용, 확인용)")]
    public bool isChasing;

    private Rigidbody2D rb;
    private int currentPointIndex;
    private IEnemyState currentState;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        currentPointIndex = 0;
        ChangeState(new PatrolState());
    }

    void Update()
    {
        currentState.Execute(this);
        UpdateDetectionVisualScale();
    }

    // ----- 상태 전환 -----
    // 상태 전환을 한 곳으로 모아두면, 상태가 늘어나도 이 메서드는 바뀌지 않는다.
    public void ChangeState(IEnemyState nextState)
    {
        currentState = nextState;
        currentState.Enter(this);
    }

    // ----- 각 State가 공통으로 사용하는 동작들 -----
    // (State는 "언제, 무엇을 할지"만 결정하고, 실제 이동/감지 계산은
    // EnemyController에 그대로 남겨 State 클래스를 얇고 단순하게 유지한다.)

    //플레이어 감지 함수
    public bool CanSeePlayer()
    {
        return Vector3.Distance(transform.position, player.position) <= detectionRange;
    }

    // 플레이어 추적 함수
    public void MoveTowardsPlayer()
    {
        Vector3 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + (Vector2)direction * chaseSpeed * Time.deltaTime);
    }

    //패트롤 동작 함수
    public void MoveTowardsCurrentPatrolPoint()
    {
        if (patrolPoints == null || patrolPoints.Count == 0)
        {
            return;
        }

        Transform target = patrolPoints[currentPointIndex];
        Vector3 direction = (target.position - transform.position).normalized;
        rb.MovePosition(rb.position + (Vector2)direction * patrolSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // 리스트의 다음 포인트로 넘어가고, 마지막이면 다시 처음으로 돌아간다.
            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
        }
    }

    // 감지 원 보이기/안보이기
    public void SetDetectionVisualActive(bool active)
    {
        if (detectionRangeVisual != null)
        {
            detectionRangeVisual.SetActive(active);
        }
    }


    //감지 원 크기 보여주는 함수
    private void UpdateDetectionVisualScale()
    {
        if (detectionRangeVisual == null)
        {
            return;
        }

        // 감지 범위 원의 반지름이 detectionRange 값과 항상 같아지도록 스케일을 맞춘다.
        SpriteRenderer visualRenderer = detectionRangeVisual.GetComponent<SpriteRenderer>();
        if (visualRenderer == null || visualRenderer.sprite == null)
        {
            return;
        }

        float nativeDiameter = visualRenderer.sprite.bounds.size.x;
        float scale = (detectionRange * 2f) / nativeDiameter;
        detectionRangeVisual.transform.localScale = new Vector3(scale, scale, 1f);
    }
}
