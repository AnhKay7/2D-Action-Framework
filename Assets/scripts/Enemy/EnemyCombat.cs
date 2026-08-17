using UnityEngine;

public class EnemyCombat : MonoBehaviour
{
    #region Component
    private Entity target;
    private GroundSensor ground;
    private AttackExecutor attackExecutor;
    #endregion

    #region HitBox 
    [SerializeField] private AttackHitBox horizontalHitBox;
    [SerializeField] private Transform combatPivot;
    #endregion

    #region Setting
    [SerializeField] private float allowAttackDistance = 2f;
    [SerializeField] private float windupTime = 0.4f;
    [SerializeField] private float activeTime = 0.1f;
    [SerializeField] private float recoveryTime = 0.15f;
    #endregion

    private void Awake()
    {
        ground = GetComponent<GroundSensor>();
        attackExecutor = new AttackExecutor();
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
        attackExecutor.TryStartAttack(horizontalHitBox, windupTime, activeTime, recoveryTime);
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
}
