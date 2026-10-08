using UnityEngine;

public class EnemyRecoveryState : EnemyState
{
    public EnemyRecoveryState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine)
    {
    }
    private float lastTimeLostTarget = -100f;
    public override void EnterState()
    {
        base.EnterState();
        lastTimeLostTarget = Time.time;
    }
    public override bool FrameUpdate()
    {
        if (base.FrameUpdate())
            return true;

        Entity target = enemy.Detection.Target;
        if (target != null)
        {
            enemy.AcquireTarget(target);
            stateMachine.ChangeState(enemy.ChaseState);
            return true;
        }
        if (Time.time > lastTimeLostTarget + enemy.GiveUpOnLostTargetTime)
        {
            enemy.ClearCurrentTarget();
            stateMachine.ChangeState(enemy.IdleState);
            return true;
        }
        return false;
    }
}
