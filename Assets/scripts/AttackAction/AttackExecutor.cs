using UnityEngine;

public class AttackExecutor
{
    private float windupTime;
    private float activeTime;
    private float recoveryTime;
    private AttackPhase attackPhase = AttackPhase.Idle;
    private float actionEndTime;
    private AttackHitBox currentHitBox;
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
        currentHitBox?.Deactivate();
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
    public bool TryStartAttack(AttackHitBox requestHitBox, float windup, float active, float recovery)
    {
        if (!CanAttack())
            return false;
        windupTime = windup;
        activeTime = active;
        recoveryTime = recovery;
        currentHitBox = requestHitBox;
        attackPhase = AttackPhase.WindUp;
        actionEndTime = Time.time + windupTime;
        return true;
    }

    private void EndAttack()
    {
        attackPhase = AttackPhase.Idle;
        currentHitBox = null;
    }
    private void StartActive()
    {
        attackPhase = AttackPhase.Active;

        currentHitBox?.Activate();

        actionEndTime = Time.time + activeTime;
    }
    private void EndActive()
    {
        currentHitBox?.Deactivate();
    }
    private void StartRecovery()
    {
        attackPhase = AttackPhase.Recovery;
        actionEndTime = Time.time + recoveryTime;
    }
    #endregion
}
