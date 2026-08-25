using UnityEngine;

public abstract class PlayerState
{
    protected Player player;
    protected PlayerStateMachine stateMachine;
    //protected string animation_name;
    public virtual bool CanAttack => true;
    public virtual bool CanReceiveHit => true;
    public virtual bool CanBeHitstunned => true;
    public PlayerState(Player _player, PlayerStateMachine _stateMachine)
    {
        this.player = _player;
        this.stateMachine = _stateMachine;
    }

    public virtual void EnterState()
    {
        //Debug.Log("hello from " + stateMachine.CurrentState.ToString());
    }

    public virtual void ExitState()
    {

    }

    public virtual bool FrameUpdate()
    {
        if (CanBeHitstunned && player.HitstunReceiver.IsHitstunned)
        {
            stateMachine.ChangeState(player.StunState);
            return true;
        }
        return false;
    }

    public virtual void PhysicUpdate()
    {
    }
    protected void ApplyGravity()
    {
        float velocityY = player.VelocityResolver.baseVelocityY;
        float gravity = Physics2D.gravity.y * player.GravityScale;

        if (velocityY < 0)
        {
            gravity *= player.FallGravityMultiplier;
        }

        velocityY += gravity * Time.fixedDeltaTime;
        velocityY = Mathf.Max(velocityY, player.MaxFallSpeed);

        player.VelocityResolver.SetBaseY(velocityY);
    }
}
