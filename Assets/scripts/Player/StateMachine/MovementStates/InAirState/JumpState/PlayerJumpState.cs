using UnityEngine;

public class PlayerJumpState : PlayerInAirState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    private bool jumpApplied;
    private bool jumpCutRequested;
    public override void EnterState()
    {
        base.EnterState();
        jumpApplied = false;
        jumpCutRequested = false;
        player.ConsumeAllJumpGraces();
    }

    private void ExecuteJump()
    {
        float targetVelocityY = Mathf.Sqrt(player.JumpHeight * -2 * (Physics2D.gravity.y * player.GravityScale));

        if (!player.Input.JumpHeld)
        {
            targetVelocityY *= player.JumpCutMultiplier;
        }

        player.VelResolver.RequestOverrideY(targetVelocityY);
    }
    private void CutJump()
    {
        float velocityY = player.Movement.velocityY;

        velocityY *= player.JumpCutMultiplier;

        player.VelResolver.RequestOverrideY(velocityY);
    }

    public override bool FrameUpdate()
    {
        if (base.FrameUpdate())
            return true;
        if (player.Input.JumpReleased)
        {
            jumpCutRequested = true;
        }
        if (jumpApplied)
        {
            if (player.Movement.velocityY <= 0f)
            {
                stateMachine.ChangeState(player.FallState);
                return true;
            }
        }
        return false;
    }
    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        if (!jumpApplied)
        {
            ExecuteJump();
            jumpApplied = true;
        }
        else if (jumpCutRequested)
        {
            jumpCutRequested = false;
            CutJump();
        }
    }
}