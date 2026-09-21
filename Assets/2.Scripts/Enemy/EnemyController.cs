using System.Collections.Generic;
using UnityEngine;

// [2차시 State Pattern - Before 코드]
// 순찰/감지/추적을 전부 조건문으로 분기하는 의도적으로 나쁜 구조의 예시.
// 상태가 늘어날수록 Update()의 if/else if 사슬이 계속 길어지는 문제를
// 다음 시간에 State Pattern으로 리팩토링해서 해결한다.
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

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        currentPointIndex = 0;
        isChasing = false;
    }

    void Update()
    {
        float distanceToPlayer = Vector3.Distance(transform.position, player.position);

        if (distanceToPlayer <= detectionRange)
        {
            // 플레이어가 찾아졌다면 추적한다.
            isChasing = true;

            Vector3 direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + (Vector2)direction * chaseSpeed * Time.deltaTime);
        }
        else if (distanceToPlayer > detectionRange)
        {
            // 플레이어가 찾아지지 않았다면 순찰한다.
            isChasing = false;

            if (patrolPoints != null && patrolPoints.Count > 0)
            {
                Transform currentTarget = patrolPoints[currentPointIndex];

                Vector3 direction = (currentTarget.position - transform.position).normalized;
                rb.MovePosition(rb.position + (Vector2)direction * patrolSpeed * Time.deltaTime);

                float distanceToTarget = Vector3.Distance(transform.position, currentTarget.position);
                if (distanceToTarget < 0.1f)
                {
                    // 리스트의 다음 포인트로 넘어가고, 마지막이면 다시 처음으로 돌아간다.
                    currentPointIndex = (currentPointIndex + 1) % patrolPoints.Count;
                }
            }
        }

        // 패트롤 중일 때만 감지 범위를 빨갛게 표시한다.
        if (detectionRangeVisual != null)
        {
            detectionRangeVisual.SetActive(!isChasing);

            // 감지 범위 원의 반지름이 detectionRange 값과 항상 같아지도록 스케일을 맞춘다.
            SpriteRenderer visualRenderer = detectionRangeVisual.GetComponent<SpriteRenderer>();
            if (visualRenderer != null && visualRenderer.sprite != null)
            {
                float nativeDiameter = visualRenderer.sprite.bounds.size.x;
                float scale = (detectionRange * 2f) / nativeDiameter;
                detectionRangeVisual.transform.localScale = new Vector3(scale, scale, 1f);
            }
        }
    }
}
