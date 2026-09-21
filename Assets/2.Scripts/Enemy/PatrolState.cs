// 순찰 상태: 정해진 지점들을 순서대로 오가고, 플레이어가 감지 범위에
// 들어오면 스스로 ChaseState로 전환한다. "언제 추적을 시작할지"를
// 이 상태 자신이 알고 있으므로, EnemyController는 그 판단에 관여하지 않는다.
public class PatrolState : IEnemyState
{
    public void Enter(EnemyController enemy)
    {
        enemy.isChasing = false;
        enemy.SetDetectionVisualActive(true);
    }

    public void Execute(EnemyController enemy)
    {
        if (enemy.CanSeePlayer())
        {
            enemy.ChangeState(new ChaseState());
            return;
        }

        enemy.MoveTowardsCurrentPatrolPoint();
    }
}
