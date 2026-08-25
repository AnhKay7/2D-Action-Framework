using UnityEngine;

public class PlayerStunState : PlayerState
{
    public PlayerStunState(Player _player, PlayerStateMachine _stateMachine) : base(_player, _stateMachine)
    {
    }
    public override bool CanAttack => false;
    public override bool CanBeHitstunned => false;
    //public override bool CanReceiveHit => true;
    public override bool FrameUpdate()
    {
        if (base.FrameUpdate())
            return true;
        if (player.HitstunReceiver.IsHitstunned == false)
        {
            if (player.Ground.IsGrounded)
            {
                if (player.Input.MoveDirection != 0f)
                    stateMachine.ChangeState(player.MoveState);
                else
                    stateMachine.ChangeState(player.IdleState);
            }
            else
            {
                stateMachine.ChangeState(player.FallState);
            }
            return true;
        }
        return false;
    }
    public override void PhysicUpdate()
    {
        base.PhysicUpdate();
        player.VelocityResolver.SetBaseX(0f);
        ApplyGravity();
    }
}
