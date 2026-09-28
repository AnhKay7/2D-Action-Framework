using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }
    private bool IsTargetOutOfRange(Entity target)
    {
        return PhysicsUtility.HorizontalDistance(enemy, enemy.CurrentTarget) > enemy.ChaseRange;
    }
    public override bool FrameUpdate()
    {
        if (base.FrameUpdate())
            return true;

        Entity target = enemy.CurrentTarget;
        if (IsTargetOutOfRange(target))
        {
            Entity newTarget = enemy.Detection.Target;
            if (newTarget == null)
            {
                stateMachine.ChangeState(enemy.RecoveryState);
                return true;
            }
            else
            {
                enemy.AcquireTarget(newTarget);
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
