using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private Transform combatPivot;
    [SerializeField] private AttackHitBox horizontalHitbox;
    [SerializeField] private AttackHitBox upHitbox;
    [SerializeField] private AttackHitBox downHitbox;
    [SerializeField] private float windupTime = 0.05f;
    [SerializeField] private float activeTime = 0.1f;
    [SerializeField] private float recoveryTime = 0.08f;
    [SerializeField] private float horizontalHitRecoilSpeed = 10f;
    [SerializeField] private float recoilDuration = 0.16f;
    [SerializeField] private float pogoLauchedHeigth = 2.5f;
    private AttackExecutor attackExecutor;
    private PlayerImpulseController impulseController;
    private float gravityScale;
    private AttackDirection currentAttackDirection;
    private float actionEndTime;
    private AttackHitBox currentHitBox;
    private enum AttackDirection
    {
        Up,
        Down,
        Right,
        Left
    }
    private void Awake()
    {
        impulseController = GetComponent<PlayerImpulseController>();
        attackExecutor = new AttackExecutor();

        horizontalHitbox.HitConfirmed += ApplyForce;
        downHitbox.HitConfirmed += ApplyForce;
    }
    public void Initialize(float GravityScale)
    {
        gravityScale = GravityScale;
    }
    public void FrameUpdate(bool stateAllowAttack)
    {
        if (attackExecutor.IsAttacking() && !stateAllowAttack)
        {
            attackExecutor.CancelAttack();
        }
        attackExecutor.FrameUpdate();
    }
    public bool CanAttack(bool stateAllowAttack)
    {
        if (!stateAllowAttack)
            return false;

        if (!attackExecutor.CanAttack())
            return false;

        return true;
    }
    public bool TryAttack(bool stateAllowAttack, int facingDirection, int attackVerticalDirection, bool isGrounded)
    {
        if (!CanAttack(stateAllowAttack))
            return false;

        if (attackVerticalDirection == 1)
        {
            currentAttackDirection = AttackDirection.Up;
            currentHitBox = upHitbox;
        }
        else if (attackVerticalDirection == -1 && !isGrounded)
        {
            currentAttackDirection = AttackDirection.Down;
            currentHitBox = downHitbox;
        }
        else
        {
            if (facingDirection == 1)
                currentAttackDirection = AttackDirection.Right;
            else
                currentAttackDirection = AttackDirection.Left;
            currentHitBox = horizontalHitbox;
        }

        UpdateCombatPivot(facingDirection);
        attackExecutor.TryStartAttack(currentHitBox, windupTime, activeTime, recoveryTime);
        return true;
    }

    private void UpdateCombatPivot(int facingDirection)
    {
        Vector3 scale = combatPivot.localScale;
        scale.x = Mathf.Abs(scale.x) * facingDirection;
        combatPivot.localScale = scale;
    }
    private void ApplyForce()
    {
        if (currentAttackDirection == AttackDirection.Right
            || currentAttackDirection == AttackDirection.Left)
        {
            int attackDirection = (currentAttackDirection == AttackDirection.Right ? 1 : -1);
            impulseController.ApplyHorizontalVelocity(-attackDirection * horizontalHitRecoilSpeed, recoilDuration);
        }

        if (currentAttackDirection == AttackDirection.Down)
        {
            impulseController.ApplyVerticalVelocity(
                PlayerPhysicsUtility.CalculateLaunchVelocity(pogoLauchedHeigth, Physics2D.gravity.y * gravityScale)
                );
        }
    }
}
