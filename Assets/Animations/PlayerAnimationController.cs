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

        if (desiredAnimation != currentAnimation)
        {
            animator.Play(desiredAnimation);
            currentAnimation = desiredAnimation;
        }

        spriteRenderer.flipX = !player.Input.IsFacingRight;
    }
}
