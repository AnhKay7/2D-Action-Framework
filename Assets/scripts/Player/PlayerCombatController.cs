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

    private float nextAttackAllowTime = -100f;
    private AttackPhase attackPhase = AttackPhase.Idle;
    private AttackDirection currentAttackDirection;
    private float actionEndTime;
    private AttackHitBox currentHitBox;
    private enum AttackPhase
    {
        Idle,
        WindUp,
        Active,
        Recovery
    }
    private enum AttackDirection
    {
        Up,
        Down,
        Horizontal
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
            currentAttackDirection = AttackDirection.Horizontal;
            currentHitBox = horizontalHitbox;
        }

        StartAttack(facingDirection);
        return true;
    }
    private void StartAttack(int facingDirection)
    {
        UpdateCombatPivot(facingDirection);

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

        currentHitBox.Activate();

        actionEndTime = Time.time + activeTime;
    }
    private void EndActive()
    {
        currentHitBox.Deactivate();
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
