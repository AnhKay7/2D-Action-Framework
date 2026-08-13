using UnityEngine;

public class PlayerWallJumpState : PlayerJumpState
{
    public PlayerWallJumpState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    public override void EnterState()
    {
        base.EnterState();
        player.VelResolver.RequestOverrideX(player.WallJumpForce * -player.Wall.WallDirection);
        wallJumpInputUnlockTime = Time.time + player.WallJumpDuration;
    }
}
