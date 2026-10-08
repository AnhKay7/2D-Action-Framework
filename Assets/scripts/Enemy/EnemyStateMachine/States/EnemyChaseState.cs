using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }
    public override bool FrameUpdate()
    {
        if (base.FrameUpdate())
            return true;

        Entity target = enemy.CurrentTarget;
        if (IsTargetOutOfRange(target, enemy.ChaseRange))
        {
            Entity detectedTarget = enemy.Detection.Target;

            if (detectedTarget != null)
            {
                enemy.AcquireTarget(detectedTarget);
                target = detectedTarget;
            }
            else
            {
                stateMachine.ChangeState(enemy.RecoveryState);
                return true;
            }
        }

        if (IsTargetInAttackRange(target))
        {
            stateMachine.ChangeState(enemy.AttackState);
            return true;
        }

        return false;
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        Entity target = enemy.CurrentTarget;

        if (target == null)
            return;

        float deltaX = target.transform.position.x - enemy.transform.position.x;

        int direction = deltaX > 0 ? 1 : -1;

        enemy.Movement.RequestMoveDirection(direction);
    }
}
