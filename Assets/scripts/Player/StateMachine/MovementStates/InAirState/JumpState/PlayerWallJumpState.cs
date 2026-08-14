using UnityEngine;

public class PlayerWallJumpState : PlayerJumpState
{
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    private bool jumpForceApplied;
    public override void EnterState()
    {
        base.EnterState();
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
