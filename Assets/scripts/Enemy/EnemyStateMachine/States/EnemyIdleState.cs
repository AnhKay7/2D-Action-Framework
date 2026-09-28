
public class EnemyIdleState : EnemyState
{
    public EnemyIdleState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }

    public override bool FrameUpdate()
    {
        if (base.FrameUpdate())
            return true;

        Entity target = enemy.Detection.Target;
        if (target == null)
            return false;

        enemy.AcquireTarget(target);

        //if (IsTargetInAttackRange(target))
        //{
        //    stateMachine.ChangeState(enemy.AttackState);
        //    return true;
        //}

        stateMachine.ChangeState(enemy.ReactState);
        return true;
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.VelocityResolver.SetBaseX(0f);
    }
}
