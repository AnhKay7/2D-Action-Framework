using UnityEngine;

public class AttackExecutor
{
    private AttackData currentAttack;
    private AttackPhase attackPhase = AttackPhase.Idle;
    private float actionEndTime;
    private enum AttackPhase
    {
        Idle,
        WindUp,
        Active,
        Recovery
    }
    public bool CanAttack()
    {

        if (attackPhase != AttackPhase.Idle)
            return false;
        //Debug.Log("YES");
        return true;
    }
    public void CancelAttack()
    {
        if (attackPhase == AttackPhase.Idle || attackPhase == AttackPhase.Recovery)
            return;
        if (attackPhase == AttackPhase.Active)
        {
            EndActive();
            StartRecovery();
            return;
        }
        currentAttack.HitBox?.Deactivate();
        EndAttack();
    }
    public bool IsAttacking()
    {
        return attackPhase != AttackPhase.Idle;
    }
    public void FrameUpdate()
    {
        if (attackPhase != AttackPhase.Idle)
        {
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
    #region Attack Cycle
    public bool TryStartAttack(AttackData attackData)
    {
        if (!CanAttack())
            return false;

        currentAttack = attackData;
        attackPhase = AttackPhase.WindUp;
        actionEndTime = Time.time + attackData.WindupTime;
        return true;
    }

    private void EndAttack()
    {
        attackPhase = AttackPhase.Idle;
        currentAttack = null;
    }
    private void StartActive()
    {
        attackPhase = AttackPhase.Active;

        currentAttack.HitBox?.Activate();

        actionEndTime = Time.time + currentAttack.ActiveTime;
    }
    private void EndActive()
    {
        currentAttack.HitBox?.Deactivate();
    }
    private void StartRecovery()
    {
        attackPhase = AttackPhase.Recovery;
        actionEndTime = Time.time + currentAttack.RecoveryTime;
    }
    #endregion
}
