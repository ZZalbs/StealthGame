// 추적 상태: 플레이어 쪽으로 이동하고, 감지 범위를 벗어나면 스스로
// PatrolState로 돌아간다. 순찰 로직과 추적 로직이 서로 다른 클래스에
// 있으므로, 추적 방식을 바꿔도 순찰 코드는 건드릴 필요가 없다.
public class ChaseState : IEnemyState
{
    public void Enter(EnemyController enemy)
    {
        enemy.isChasing = true;
        enemy.SetDetectionVisualActive(false);
    }

    public void Execute(EnemyController enemy)
    {
        if (!enemy.CanSeePlayer())
        {
            enemy.ChangeState(new PatrolState());
            return;
        }

        enemy.MoveTowardsPlayer();
    }
}
