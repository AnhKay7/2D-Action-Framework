using UnityEngine;

public abstract class EnemyState
{
    protected Enemy enemy;
    protected EnemyStateMachine stateMachine;

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
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual bool FrameUpdate() {

        return false;
    }
    public virtual void PhysicsUpdate() { }
}
