using UnityEngine;

public class PlayerCombatController : MonoBehaviour
{
    [SerializeField] private Transform combatPivot;
    [SerializeField] private AttackHitBox horizontalHitbox;
    [SerializeField] private float windupTime = 0.05f;
    [SerializeField] private float activeTime = 0.1f;
    [SerializeField] private float recoveryTime = 0.08f;

    private int attackDirection;
    private float nextAttackAllowTime = -100f;
    private AttackPhase attackPhase = AttackPhase.Idle;
    private float actionEndTime;

    private enum AttackPhase
    {
        Idle,
        WindUp,
        Active,
        Recovery
    }

    public void FrameUpdate(int facingDirection, bool stateAllowAttack)
    {
        if (attackPhase != AttackPhase.Idle)
        {
            if (!stateAllowAttack)
            {
                EndAttack();
                return;
            }
            if (Time.time >= actionEndTime)
            {
                if (attackPhase == AttackPhase.WindUp)
                {
                    StartActive();
                    return;
                }
                if (attackPhase == AttackPhase.Active)
                {
                    EndActive();
                    StartRecovery();
                    return;
                }
                if (attackPhase == AttackPhase.Recovery)
                {
                    EndAttack();
                    return;
                }
            }
        }
    }
    public bool CanAttack(bool stateAllowAttack)
    {
        if (!stateAllowAttack)
            return false;

        if (attackPhase != AttackPhase.Idle)
            return false;

        if (Time.time < nextAttackAllowTime)
            return false;

        return true;
    }
    public void TryAttack(bool stateAllowAttack, int facingDirection)
    {
        if (!CanAttack(stateAllowAttack))
            return;

        StartAttack(facingDirection);
    }
    private void StartAttack(int facingDirection)
    {
        UpdateCombatPivot(facingDirection);
        attackDirection = facingDirection;

        attackPhase = AttackPhase.WindUp;

        actionEndTime = Time.time + windupTime;
        nextAttackAllowTime = Time.time + windupTime + activeTime + recoveryTime;
    }

    private void EndAttack()
    {
        attackPhase = AttackPhase.Idle;
        EndActive();
    }
    private void StartActive()
    {
        attackPhase = AttackPhase.Active;
        horizontalHitbox.Activate();
        actionEndTime = Time.time + activeTime;
    }
    private void EndActive()
    {
        horizontalHitbox.Deactivate();
    }
    private void StartRecovery()
    {
        attackPhase = AttackPhase.Recovery;
        actionEndTime = Time.time + recoveryTime;
    }
    private void UpdateCombatPivot(int facingDirection)
    {
        Vector3 scale = combatPivot.localScale;
        scale.x = Mathf.Abs(scale.x) * facingDirection;
        combatPivot.localScale = scale;
    }
}
