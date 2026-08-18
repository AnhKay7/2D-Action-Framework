using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    #region Config
    [Header("Hitbox")]
    [SerializeField] private Transform combatPivot;
    [SerializeField] private AttackData horizontalAttack;
    [SerializeField] private AttackData upAttack;
    [SerializeField] private AttackData downAttack;

    [Header("Player Hit Response")]
    [SerializeField] private float horizontalHitRecoilSpeed = 10f;
    [SerializeField] private float recoilDuration = 0.16f;
    [SerializeField] private float pogoLauchedHeigth = 2.5f;
    #endregion

    #region Variable & component
    private Entity owner;
    private AttackExecutor attackExecutor;
    private AttackEffectProcessor attackEffectProcessor;
    private ImpulseController impulseController;
    private float gravityScale;
    private AttackData currentAttack;
    #endregion
    private void Awake()
    {
        owner = GetComponentInParent<Entity>();
        attackEffectProcessor = new AttackEffectProcessor();
        attackExecutor = new AttackExecutor();

        horizontalAttack.HitBox.HitConfirmed += HandleTargetHit;
        downAttack.HitBox.HitConfirmed += HandleTargetHit;
        upAttack.HitBox.HitConfirmed += HandleTargetHit;
    }
    public void Initialize(float GravityScale, ImpulseController impulseController)
    {
        gravityScale = GravityScale;
        this.impulseController = impulseController;
    }
    public void FrameUpdate(bool stateAllowAttack)
    {
        if (attackExecutor.IsAttacking() && !stateAllowAttack)
        {
            attackExecutor.CancelAttack();
        }
        attackExecutor.FrameUpdate();
    }

    #region Attack lifecycle
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
            currentAttack = upAttack;
        }
        else if (attackVerticalDirection == -1 && !isGrounded)
        {
            currentAttack = downAttack;
        }
        else
        {
            currentAttack = horizontalAttack;
        }

        UpdateCombatPivot(facingDirection);
        attackExecutor.TryStartAttack(currentAttack);
        return true;
    }

    private void UpdateCombatPivot(int facingDirection)
    {
        Vector3 scale = combatPivot.localScale;
        scale.x = Mathf.Abs(scale.x) * facingDirection;
        combatPivot.localScale = scale;
    }
    #endregion

    #region Hit Response
    private void HandleTargetHit(Entity target)
    {
        attackEffectProcessor.ApplyAttackEffect(owner, target, currentAttack);
        ApplySelfResponse(target);
    }
    private void ApplySelfResponse(Entity target)
    {
        if (currentAttack == horizontalAttack)
        {
            float deltaX = target.transform.position.x - transform.position.x;
            int attackDirection = (deltaX > 0 ? 1 : -1);
            impulseController.ApplyHorizontalOverrideVelocity(-attackDirection * horizontalHitRecoilSpeed, recoilDuration);
        }

        if (currentAttack == downAttack)
        {
            impulseController.ApplyVerticalOverrideVelocityOneshot(
                PlayerPhysicsUtility.CalculateLaunchVelocity(pogoLauchedHeigth, Physics2D.gravity.y * gravityScale)
                );
        }
    }
    #endregion

}
