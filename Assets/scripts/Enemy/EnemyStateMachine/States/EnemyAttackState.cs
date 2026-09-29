using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }
    public override bool CanTurn => false;
    public override void EnterState()
    {
        base.EnterState();
        enemy.Combat.SetTarget(enemy.CurrentTarget);
    }
    public override bool FrameUpdate()
    {
        if (base.FrameUpdate())
            return true;

        Entity target = enemy.CurrentTarget;
        if (enemy.Combat.IsAttacking)
        {
            return false;
        }
        if (!IsTargetOutOfRange(target, enemy.Combat.AttackStartRange))
        {
            return false;
        }
        if (!IsTargetOutOfRange(target, enemy.ChaseRange))
        {
            stateMachine.ChangeState(enemy.ChaseState);
            return true;
        }
        stateMachine.ChangeState(enemy.RecoveryState);
        return true;
    }
    public override void ExitState()
    {
        base.ExitState();
        enemy.Combat.ResetTarget();
    }
}
