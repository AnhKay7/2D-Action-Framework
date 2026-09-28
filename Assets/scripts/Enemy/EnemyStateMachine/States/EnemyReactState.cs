using UnityEngine;

public class EnemyReactState : EnemyState
{
    float lastReactTime = -100f;
    public EnemyReactState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }
    public override void EnterState()
    {
        base.EnterState();
        lastReactTime = Time.time;
    }
    public override bool FrameUpdate()
    {
        if (base.FrameUpdate())
            return true;

        Entity target = enemy.CurrentTarget;
        if (IsTargetInAttackRange(target))
        {
            stateMachine.ChangeState(enemy.AttackState);
            return true;
        }

        if (Time.time > lastReactTime + enemy.ReactTime)
        {
            stateMachine.ChangeState(enemy.ChaseState);
            return true;
        }
        return false;
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        enemy.VelocityResolver.SetBaseX(0f);
    }
}
