using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public abstract class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;
    public virtual bool CanTurn => true;

    protected EnemyState(Enemy enemy, EnemyStateMachine stateMachine)
    {
        this.enemy = enemy;
        this.stateMachine = stateMachine;
    }
    protected bool IsTargetInAttackRange(Entity target)
    {
        if (target == null)
            return false;

        return PhysicsUtility.HorizontalDistance(enemy, target) <= enemy.Combat.AttackStartRange;
    }
    protected bool IsTargetOutOfRange(Entity target, float range)
    {
        if (target == null)
            return true;
        return PhysicsUtility.HorizontalDistance(enemy, target) > range;
    }
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual bool FrameUpdate() {

        return false;
    }
    public virtual void PhysicsUpdate() { }
}
