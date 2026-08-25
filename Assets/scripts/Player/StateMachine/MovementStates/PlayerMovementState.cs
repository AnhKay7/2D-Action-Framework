using UnityEngine;

public class PlayerMovementState : PlayerState
{
    public PlayerMovementState(Player _player, PlayerStateMachine _stateMachine) : base(_player, _stateMachine)
    {
    }
    protected virtual bool AllowDash => true;
    protected bool CheckDashTransition()
    {
        if (AllowDash && player.Input.DashInput && player.DashController.CanDash(player.Ground.IsGrounded))
        {
            stateMachine.ChangeState(player.DashState);
            return true;
        }
        return false;
    }
    public override bool FrameUpdate()
    {
        if (base.FrameUpdate())
            return true;
        if (CheckDashTransition())
        {
            return true;
        }
        return false;
    }
    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        ApplyGravity();
    }
}
