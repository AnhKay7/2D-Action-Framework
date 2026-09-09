using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Player player;
    private Animator animator;
    private string currentAnimation = "PlayerIdle";
    private SpriteRenderer spriteRenderer;
    private float attackSwingAnimationEndTime = -100f;
    private float attackAnimationEndTime = -100f;
    private string requestBaseAnimation = null;
    private string requestOverrideAnimation = null;
    private string requestDominantAnimation = null;
    private bool isAttackDirectionFacingRight = true;
    private bool isPlayingAttackAnimation = false;
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void RequestAttackAnimation(AttackData attackData, int attackVerticalDirection, int facingDirection)
    {
        //Attack controller must make sure that owner can perform attack
        attackSwingAnimationEndTime = Time.time + attackData.WindupTime + attackData.ActiveTime;
        attackAnimationEndTime = attackSwingAnimationEndTime + attackData.RecoveryTime;
        isAttackDirectionFacingRight = (facingDirection == 1 ? true : false);
        if (attackVerticalDirection == 1)
            requestOverrideAnimation = "PlayerUpAttack";
        else if (attackVerticalDirection == -1 && !player.Ground.IsGrounded)
            requestOverrideAnimation = "PlayerDownAttack";
        else
            requestOverrideAnimation = "PlayerAttack";
        isPlayingAttackAnimation = true;
    }
    private void ResolveAnimation()
    {
        if (requestDominantAnimation != null)
            currentAnimation = requestDominantAnimation;
        else if (requestOverrideAnimation != null)
            currentAnimation = requestOverrideAnimation;
        else
        {
            if (currentAnimation == "PlayerAttack" || currentAnimation == "PlayerUpAttack" || currentAnimation == "PlayerDownAttack")
            {
                if (Time.time >= attackSwingAnimationEndTime)
                {
                    if (Time.time >= attackAnimationEndTime || requestBaseAnimation != "PlayerIdle")
                        currentAnimation = requestBaseAnimation;
                }
            }
            else currentAnimation = requestBaseAnimation;
        }

        if (currentAnimation != "PlayerAttack" && currentAnimation != "PlayerUpAttack" && currentAnimation != "PlayerDownAttack")
            isPlayingAttackAnimation = false;
    }
    private void ResetRequest()
    {
        requestBaseAnimation = null;
        requestOverrideAnimation = null;
        requestDominantAnimation = null;
    }
    private void LateUpdate()
    {
        if (player.StateMachine.CurrentState == player.IdleState)
            requestBaseAnimation = "PlayerIdle";
        if (player.StateMachine.CurrentState == player.MoveState)
            requestBaseAnimation = "PlayerRun";

        if (player.StateMachine.CurrentState == player.FallState)
            requestBaseAnimation = "PlayerFall";
        if (player.StateMachine.CurrentState == player.JumpState)
            requestBaseAnimation = "PlayerJump";
        if (player.StateMachine.CurrentState == player.WallSlideState)
            requestBaseAnimation = "PlayerFall";
        if (player.StateMachine.CurrentState == player.WallJumpState)
            requestBaseAnimation = "PlayerJump";

        if (player.StateMachine.CurrentState == player.DashState)
            requestDominantAnimation = "PlayerDash";
        if (player.StateMachine.CurrentState == player.StunState)
            requestDominantAnimation = "PlayerHurt";

        ResolveAnimation();
        animator.Play(currentAnimation);

        bool playerFacingDirection = player.Input.IsFacingRight;

        if (player.StateMachine.CurrentState == player.WallSlideState)
            playerFacingDirection = !playerFacingDirection;
        if (isPlayingAttackAnimation)
            playerFacingDirection = isAttackDirectionFacingRight;

        spriteRenderer.flipX = !playerFacingDirection;
        ResetRequest();
    }
}
