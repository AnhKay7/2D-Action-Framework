using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    #region Component
    private Entity owner;
    private Entity target;
    private GroundSensor ground;
    private AttackExecutor attackExecutor;
    private AttackEffectProcessor attackEffectProcessor;
    #endregion

    #region HitBox 
    [SerializeField] private AttackData horizontalAttack;
    [SerializeField] private Transform combatPivot;
    #endregion

    #region Setting
    [SerializeField] private float allowAttackDistance = 2f;

    #endregion

    private void Awake()
    {
        owner = GetComponentInParent<Entity>();
        ground = GetComponent<GroundSensor>();
        attackExecutor = new AttackExecutor();
        attackEffectProcessor = new AttackEffectProcessor();

        horizontalAttack.HitBox.HitConfirmed += HandleTargetHit;
    }
    public void SetTarget(Entity target)
    {
        this.target = target;
    }
    private void UpdateCombatPivot(int facingDirection)
    {
        Vector3 scale = combatPivot.localScale;
        scale.x = Mathf.Abs(scale.x) * facingDirection;
        combatPivot.localScale = scale;
    }
    private void TryAttack()
    {
        if (!attackExecutor.CanAttack())
            return;
        if (!ground.IsGrounded)
            return;
        float deltaX = target.transform.position.x - transform.position.x;
        int attackDirection = deltaX > 0 ? 1 : -1;
        UpdateCombatPivot(attackDirection);
        attackExecutor.TryStartAttack(horizontalAttack);
    }
    public void FrameUpdate(bool stateAllowAttack = true)
    {
        if (!stateAllowAttack)
        {
            if (attackExecutor.IsAttacking())
                attackExecutor.CancelAttack();
        }
        else if (target != null)
        {
            float distance = Mathf.Abs(target.transform.position.x - transform.position.x);
            if (distance <= allowAttackDistance)
                TryAttack();
        }
        attackExecutor.FrameUpdate();
    }
    public void PhysicsUpdate(VelocityResolver velocityResolver)
    {
        if (attackExecutor.IsAttacking())
        {
            velocityResolver.RequestOverrideX(0f);
        }
    }
    private void HandleTargetHit(Entity target)
    {
        attackEffectProcessor.ApplyAttackEffect(owner, target, horizontalAttack);
    }
}
