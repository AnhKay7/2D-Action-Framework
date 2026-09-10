using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Player player;
    private Animator animator;
    private PlayerAnimation currentAnimation = PlayerAnimation.Idle;
    private SpriteRenderer spriteRenderer;
    private float attackSwingAnimationEndTime = -100f;
    private float attackAnimationEndTime = -100f;
    private PlayerAnimation? requestBaseAnimation = null;
    private PlayerAnimation? requestOverrideAnimation = null;
    private PlayerAnimation? requestDominantAnimation = null;
    private PlayerAnimation? terminalAnimation = null;
    private bool isAttackDirectionFacingRight = true;
    private bool isHurtDirectionFacingRight = true;
    public event Action DeathAnimationFinish;
    private enum PlayerAnimation{

        Idle,
        Run,
        Jump,
        Fall,

        Attack,
        UpAttack,
        DownAttack,

        Dash,
        Hurt,
        Death
    }
    private static class AnimationHash
    {
        public static readonly int Idle = Animator.StringToHash("PlayerIdle");
        public static readonly int Run = Animator.StringToHash("PlayerRun");
        public static readonly int Fall = Animator.StringToHash("PlayerFall");
        public static readonly int Jump = Animator.StringToHash("PlayerJump");
        public static readonly int Attack = Animator.StringToHash("PlayerAttack");
        public static readonly int UpAttack = Animator.StringToHash("PlayerUpAttack");
        public static readonly int DownAttack = Animator.StringToHash("PlayerDownAttack");
        public static readonly int Dash = Animator.StringToHash("PlayerDash");
        public static readonly int Hurt = Animator.StringToHash("PlayerHurt");
        public static readonly int Death = Animator.StringToHash("PlayerDeath");
    }
    private int GetAnimationHash(PlayerAnimation animation)
    {
        return animation switch
        {
            PlayerAnimation.Idle => AnimationHash.Idle,
            PlayerAnimation.Run => AnimationHash.Run,
            PlayerAnimation.Fall => AnimationHash.Fall,
            PlayerAnimation.Jump => AnimationHash.Jump,
            PlayerAnimation.Attack => AnimationHash.Attack,
            PlayerAnimation.UpAttack => AnimationHash.UpAttack,
            PlayerAnimation.DownAttack => AnimationHash.DownAttack,
            PlayerAnimation.Dash => AnimationHash.Dash,
            PlayerAnimation.Hurt => AnimationHash.Hurt,
            PlayerAnimation.Death => AnimationHash.Death,
            _ => AnimationHash.Idle
        };
    }
    private void Awake()
    {
        player = GetComponentInParent<Player>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private bool IsAttackAnimation(PlayerAnimation targetAnimation)
    {
        return targetAnimation is
            PlayerAnimation.Attack
            or PlayerAnimation.UpAttack
            or PlayerAnimation.DownAttack;
    }
    private bool IsTerminalAnimationFinished()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        int expectedHash = GetAnimationHash(terminalAnimation.Value);

        return !animator.IsInTransition(0)
            && state.shortNameHash == expectedHash
            && state.normalizedTime >= 1f;
    }
    public void RequestAttackAnimation(AttackData attackData, int attackVerticalDirection, int facingDirection)
    {
        //Attack controller must make sure that owner can perform attack
        attackSwingAnimationEndTime = Time.time + attackData.WindupTime + attackData.ActiveTime;
        attackAnimationEndTime = attackSwingAnimationEndTime + attackData.RecoveryTime;
        isAttackDirectionFacingRight = (facingDirection == 1 ? true : false);
        if (attackVerticalDirection == 1)
            requestOverrideAnimation = PlayerAnimation.UpAttack;
        else if (attackVerticalDirection == -1 && !player.Ground.IsGrounded)
            requestOverrideAnimation = PlayerAnimation.DownAttack;
        else
            requestOverrideAnimation = PlayerAnimation.Attack;
    }
    public void RequestDeathAnimation()
    {
        terminalAnimation = PlayerAnimation.Death;
    }
    public void RequestHurtAnimation(int facingDirection)
    {
        isHurtDirectionFacingRight = (facingDirection >= 0);
    }
    private PlayerAnimation ResolveAnimation(PlayerAnimation currentAnimaiton)
    {
        PlayerAnimation baseAnimation = requestBaseAnimation ?? PlayerAnimation.Idle;
        if (requestDominantAnimation != null)
            currentAnimaiton = requestDominantAnimation.Value;
        else if (requestOverrideAnimation != null)
            currentAnimaiton = requestOverrideAnimation.Value;
        else
        {
            if (IsAttackAnimation(currentAnimaiton))
            {
                if (Time.time >= attackSwingAnimationEndTime)
                {
                    if (Time.time >= attackAnimationEndTime || baseAnimation != PlayerAnimation.Idle)
                        currentAnimaiton = baseAnimation;
                }
            }
            else currentAnimaiton = baseAnimation;
        }
        return currentAnimaiton;
    }
    private void ResetRequest()
    {
        requestBaseAnimation = null;
        requestOverrideAnimation = null;
        requestDominantAnimation = null;
    }
    private void ChangeAnimation(PlayerAnimation nextAnimation)
    {
        if (currentAnimation == nextAnimation)
        {
            if (!IsAttackAnimation(currentAnimation))
                return;
        }

        currentAnimation = nextAnimation;
        animator.Play(GetAnimationHash(currentAnimation));
    }
    private void LateUpdate()
    {
        if (terminalAnimation != null)
        {
            ChangeAnimation(terminalAnimation.Value);

            if (IsTerminalAnimationFinished())
            {
                if (terminalAnimation == PlayerAnimation.Death)
                {
                    terminalAnimation = null;
                    DeathAnimationFinish?.Invoke();
                    return;
                }
                terminalAnimation = null;
            }
            else
                return;
        }
        
        if (player.StateMachine.CurrentState == player.IdleState)
            requestBaseAnimation = PlayerAnimation.Idle;
        if (player.StateMachine.CurrentState == player.MoveState)
            requestBaseAnimation = PlayerAnimation.Run;

        if (player.StateMachine.CurrentState == player.FallState)
            requestBaseAnimation = PlayerAnimation.Fall;
        if (player.StateMachine.CurrentState == player.JumpState)
            requestBaseAnimation = PlayerAnimation.Jump;
        if (player.StateMachine.CurrentState == player.WallSlideState)
            requestBaseAnimation = PlayerAnimation.Fall;
        if (player.StateMachine.CurrentState == player.WallJumpState)
            requestBaseAnimation = PlayerAnimation.Jump;

        if (player.StateMachine.CurrentState == player.DashState)
            requestDominantAnimation = PlayerAnimation.Dash;
        if (player.StateMachine.CurrentState == player.StunState)
            requestDominantAnimation = PlayerAnimation.Hurt;

        PlayerAnimation nextAnimation = ResolveAnimation(currentAnimation);
        ChangeAnimation(nextAnimation);

        bool playerFacingDirection = player.IsFacingRight;

        if (player.StateMachine.CurrentState == player.WallSlideState)
            playerFacingDirection = !playerFacingDirection;
        if (IsAttackAnimation(currentAnimation))
            playerFacingDirection = isAttackDirectionFacingRight;
        if (currentAnimation == PlayerAnimation.Hurt)
            playerFacingDirection = isHurtDirectionFacingRight;
        spriteRenderer.flipX = !playerFacingDirection;
        ResetRequest();
    }
}
