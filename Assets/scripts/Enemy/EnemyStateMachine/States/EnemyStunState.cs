using UnityEngine;

public class EnemyStunState : EnemyState
{
    public EnemyStunState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }
    public override bool CanTurn => false;
    public override bool FrameUpdate()
    {
        if (base.FrameUpdate())
            return true;
        if (enemy.HitstunReceiver.IsHitstunned == false)
        {
            Entity target = enemy.CurrentTarget;

            if (IsTargetInAttackRange(target))
            {
                stateMachine.ChangeState(enemy.AttackState);
                return true;
            }
            if (!IsTargetOutOfRange(target, enemy.ChaseRange))
            {
                stateMachine.ChangeState(enemy.ChaseState);
                return true;
            }
            stateMachine.ChangeState(enemy.RecoveryState);
            return true;
        }
        return false;
    }
}
