using UnityEngine;

public class EnemyDeathState : EnemyState
{
    public EnemyDeathState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }
    public override bool CanReceiveHit => false;
    public override bool CanTurn => false;

    public override void EnterState()
    {
        base.EnterState();

        enemy.Combat.CancelAttack();
        enemy.ClearCurrentTarget();
        enemy.RequestDeathAnimation();
    }
}
