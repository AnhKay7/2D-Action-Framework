using UnityEngine;

public class PlayerWallJumpState : PlayerJumpState
{
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    private bool jumpForceApplied;
    public override bool CanTurn => Time.time >= wallJumpInputUnlockTime;
    public override void EnterState()
    {
        base.EnterState();
        player.SetFacingDirection(-player.Wall.WallDirection);
        jumpForceApplied = false;
        wallJumpInputUnlockTime = Time.time + player.WallJumpDuration;
    }
    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        if (!jumpForceApplied)
        {
            jumpForceApplied = true;
            player.VelocityResolver.RequestOverrideX(player.WallJumpForce * -player.Wall.WallDirection);
        }
    }
}
