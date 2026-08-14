using UnityEngine;

public class PlayerJumpState : PlayerInAirState
{
    public PlayerJumpState(Player player, PlayerStateMachine stateMachine) : base(player, stateMachine)
    {
    }

    private bool jumpApplied;
    private bool jumpCutRequested;
    private bool jumpCutApplied;
    public override void EnterState()
    {
        base.EnterState();
        jumpApplied = false;
        jumpCutRequested = false;
        jumpCutApplied = false;
        player.ConsumeAllJumpGraces();
    }

    private void ExecuteJump()
    {
        float targetVelocityY = PlayerPhysicsUtility.CalculateLaunchVelocity(player.JumpHeight, Physics2D.gravity.y * player.GravityScale);
        
        if (!player.Input.JumpHeld)
        {
            jumpCutApplied = true;
            targetVelocityY *= player.JumpCutMultiplier;
        }

        player.VelocityResolver.RequestOverrideY(targetVelocityY);
    }
    private void CutJump()
    {
        float velocityY = player.VelocityResolver.baseVelocityY;

        velocityY *= player.JumpCutMultiplier;
        jumpCutApplied = true;

        if (velocityY > 0)
            player.VelocityResolver.RequestOverrideY(velocityY);
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
        else if (jumpCutRequested && !jumpCutApplied)
        {
            jumpCutRequested = false;
            CutJump();
        }
    }
}