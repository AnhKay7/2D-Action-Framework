using System;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Enemy enemy;
    private EnemyCombat combat;
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private EnemyAnimation currentAnimation = EnemyAnimation.Idle;
    private EnemyAnimation? requestBaseAnimation = null;
    private EnemyAnimation? requestOverrideAnimation = null;
    private EnemyAnimation? requestDominantAnimation = null;
    private EnemyAnimation? terminalAnimation = null;
    public event Action DeathAnimationFinish;
    private bool requestOverrideRestart = false;
    private bool attackRestartRequested = false;
    private enum EnemyAnimation
    {
        Idle, 
        Walk,
        Attack,
        React,
        Hurt,
        Death
    }
    private static class AnimationHash
    {
        public static readonly int Idle = Animator.StringToHash("EnemyIdle");
        public static readonly int Walk = Animator.StringToHash("EnemyWalk");
        public static readonly int Attack = Animator.StringToHash("EnemyAttack");
        public static readonly int React = Animator.StringToHash("EnemyReact");
        public static readonly int Hurt = Animator.StringToHash("EnemyHurt");
        public static readonly int Death = Animator.StringToHash("EnemyDeath");
    }
    private int GetAnimationHash(EnemyAnimation animation)
    {
        return animation switch
        {
            EnemyAnimation.Idle => AnimationHash.Idle,
            EnemyAnimation.Walk => AnimationHash.Walk,
            EnemyAnimation.Attack => AnimationHash.Attack,
            EnemyAnimation.React => AnimationHash.React,
            EnemyAnimation.Hurt => AnimationHash.Hurt,
            EnemyAnimation.Death => AnimationHash.Death,
            _ => AnimationHash.Idle
        };
    }
    private void Awake()
    {
        enemy = GetComponentInParent<Enemy>();
        combat = GetComponentInParent<EnemyCombat>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    private void OnEnable()
    {
        combat.AttackStarted += HandleAttackStarted;
    }
    private void OnDisable()
    {
        combat.AttackStarted -= HandleAttackStarted;
    }
    private void ResetRequest()
    {
        requestBaseAnimation = null;
        requestOverrideAnimation = null;
        requestDominantAnimation = null;
        requestOverrideRestart = false;
        attackRestartRequested = false;
    }
    private EnemyAnimation ResolveAnimation(EnemyAnimation currentAnimaiton, out bool restart)
    {
        restart = false;
        EnemyAnimation baseAnimation = requestBaseAnimation ?? EnemyAnimation.Idle;
        if (requestDominantAnimation != null)
            currentAnimaiton = requestDominantAnimation.Value;
        else if (requestOverrideAnimation != null)
        {
            currentAnimaiton = requestOverrideAnimation.Value;
            restart = requestOverrideRestart;
        }
        else
        {
            currentAnimaiton = baseAnimation;
        }
        return currentAnimaiton;
    }
    private bool IsTerminalAnimationFinished()
    {
        AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

        int expectedHash = GetAnimationHash(terminalAnimation.Value);

        return !animator.IsInTransition(0)
            && state.shortNameHash == expectedHash
            && state.normalizedTime >= 1f;
    }
    public void RequestDeathAnimation()
    {
        ResetRequest();
        terminalAnimation = EnemyAnimation.Death;
    }
    private void ChangeAnimation(EnemyAnimation nextAnimation, bool restart = false)
    {
        if (currentAnimation == nextAnimation && !restart)
        {
            return;
        }

        currentAnimation = nextAnimation;
        animator.Play(GetAnimationHash(currentAnimation), 0, 0f);
    }
    private void HandleAttackStarted(AttackData attack, int direction)
    {
        attackRestartRequested = true;
    }
    private void LateUpdate()
    {
        if (terminalAnimation != null)
        {
            ChangeAnimation(terminalAnimation.Value);

            if (IsTerminalAnimationFinished())
            {
                if (terminalAnimation == EnemyAnimation.Death)
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
        if (enemy.StateMachine.CurrentState == enemy.IdleState)
            requestBaseAnimation = EnemyAnimation.Idle;
        if (enemy.StateMachine.CurrentState == enemy.ChaseState)
            requestBaseAnimation = EnemyAnimation.Walk;
        if (enemy.StateMachine.CurrentState == enemy.ReactState)
            requestBaseAnimation = EnemyAnimation.React;
        if (enemy.StateMachine.CurrentState == enemy.StunState)
            requestBaseAnimation = EnemyAnimation.Hurt;
        if (combat.IsAttacking)
        {
            requestOverrideAnimation = EnemyAnimation.Attack;
            requestOverrideRestart = attackRestartRequested;
        }

        EnemyAnimation nextAnimation = ResolveAnimation(currentAnimation, out bool restart);
        ChangeAnimation(nextAnimation, restart);

        bool enemyFacingDirection = enemy.IsFacingRight;
        spriteRenderer.flipX = !enemyFacingDirection;

        ResetRequest();
    }
}
