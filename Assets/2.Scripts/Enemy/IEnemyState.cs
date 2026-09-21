// [2차시 State Pattern - After 코드]
// 상태 하나당 "이 상태에서 무엇을 하는지"와
// "이 상태로 막 들어왔을 때 한 번만 할 일"만 책임진다.
// EnemyController는 더 이상 if/else로 행동을 분기하지 않고,
// 현재 상태 객체에게 실행을 위임하기만 하면 된다.
public interface IEnemyState
{
    // 상태에 진입한 첫 프레임에 한 번만 실행된다.
    // (예: 감지 범위 시각화를 켜고 끄는 것처럼, 매 프레임 반복할 필요 없는 처리)
    void Enter(EnemyController enemy);

    // 이 상태로 있는 동안 매 프레임 실행된다.
    void Execute(EnemyController enemy);
}
