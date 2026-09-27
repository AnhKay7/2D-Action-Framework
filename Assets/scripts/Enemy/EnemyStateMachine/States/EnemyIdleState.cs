
public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override bool FrameUpdate()
    {
        if (base.FrameUpdate())
            return true;
        return false;
    }
}
