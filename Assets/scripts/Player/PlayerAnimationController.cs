using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Player player;
    private Animator animator;
    private string currentAnimation = "PlayerIdle";
    private SpriteRenderer spriteRenderer;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void LateUpdate()
    {
        string desiredAnimation = currentAnimation;
        if (player.StateMachine.CurrentState == player.IdleState)
            desiredAnimation = "PlayerIdle";
        if (player.StateMachine.CurrentState == player.MoveState)
            desiredAnimation = "PlayerRun";
        if (player.StateMachine.CurrentState == player.FallState)
            desiredAnimation = "PlayerFall";
        if (player.StateMachine.CurrentState == player.JumpState)
            desiredAnimation = "PlayerJump";
        if (player.StateMachine.CurrentState == player.DashState)
            desiredAnimation = "PlayerDash";
        if (player.StateMachine.CurrentState == player.WallSlideState)
            desiredAnimation = "PlayerFall";
        if (player.StateMachine.CurrentState == player.WallJumpState)
            desiredAnimation = "PlayerJump";
        if (player.StateMachine.CurrentState == player.StunState)
            desiredAnimation = "PlayerHurt";
        if (desiredAnimation != currentAnimation)
        {
            animator.Play(desiredAnimation);
            currentAnimation = desiredAnimation;
        }

        bool playerFacingDirection = player.Input.IsFacingRight;

        if (player.StateMachine.CurrentState == player.WallSlideState)
            playerFacingDirection = !playerFacingDirection;

        spriteRenderer.flipX = !playerFacingDirection;
    }
}
