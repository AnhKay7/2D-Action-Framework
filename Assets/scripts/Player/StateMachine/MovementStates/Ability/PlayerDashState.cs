using UnityEngine;

public class PlayerDashState : PlayerAbilityState
{
    protected override bool AllowDash => false;
    public override bool CanAttack => false;
    public override bool CanTurn => false;
    private float dashDirection;
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine) : base(_player, _stateMachine)
    {
    }
    public void SetDashDirection(float DashDirection)
    {
        dashDirection = DashDirection;
    }
    public override void EnterState()
    {
        base.EnterState();
        player.Input.UseDashInput();
        player.SetFacingDirection((int)dashDirection);

        player.DashController.ConsumeDash(player.Ground.IsGrounded);
        abilityEndTime = Time.time + player.DashController.DashDuration;
    }
    public override bool FrameUpdate()
    {
        if (base.FrameUpdate())
            return true;
        if (Time.time >= abilityEndTime)
        {
            if (player.Ground.IsGrounded)
            {
                if (player.Input.MoveDirection != 0)
                    stateMachine.ChangeState(player.MoveState);
                else
                    stateMachine.ChangeState(player.IdleState);
                return true;
            }

            if (player.Wall.IsTouchingWall)
            {
                stateMachine.ChangeState(player.WallSlideState);
                return true;
            }
            stateMachine.ChangeState(player.FallState);
            return true;
        }
        return false;
    }
    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        player.VelocityResolver.RequestDominant(dashDirection * player.DashController.DashSpeed, 0f);
    }
}
