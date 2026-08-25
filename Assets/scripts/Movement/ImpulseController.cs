using UnityEngine;

public class ImpulseController
{
    #region Override Variable
    private float? oneshotOverrideY = null;
    private float? RequestOverrideVelocityX = null;
    private float? RequestOverrideVelocityY = null;
    private float RequestXOverrideEndTime;
    private float RequestYOverrideEndTime;
    public void ApplyHorizontalOverrideVelocity(float velocityX, float duration)
    {
        RequestOverrideVelocityX = velocityX;
        RequestXOverrideEndTime = Time.time + duration;
    }
    public void ApplyVerticalOverrideVelocity(float velocityY, float duration)
    {
        RequestOverrideVelocityY = velocityY;
        RequestYOverrideEndTime = Time.time + duration;
    }
    public void ApplyVelocityOverride(float velocityX, float velocityY, float duration)
    {
        RequestOverrideVelocityX = velocityX;
        RequestOverrideVelocityY = velocityY;
        RequestXOverrideEndTime = Time.time + duration;
        RequestYOverrideEndTime = Time.time + duration;
    }
    public void ApplyVerticalOverrideVelocityOneshot(float VelocityY)
    {
        oneshotOverrideY = VelocityY;
    }
    public void CancelImpulseOverride()
    {
        RequestOverrideVelocityX = null;
        RequestOverrideVelocityY = null;
        RequestXOverrideEndTime = -100f;
        RequestYOverrideEndTime = -100f;
    }
    #endregion

    #region Dominant Variable
    private float? oneshotDominantY = null;
    private float? RequestDominantVelocityX = null;
    private float? RequestDominantVelocityY = null;
    private float RequestXDominantEndTime;
    private float RequestYDominantEndTime;
    public void ApplyHorizontalDominantVelocity(float velocityX, float duration)
    {
        RequestDominantVelocityX = velocityX;
        RequestXDominantEndTime = Time.time + duration;
    }
    public void ApplyVerticalDominantVelocity(float velocityY, float duration)
    {
        RequestDominantVelocityY = velocityY;
        RequestYDominantEndTime = Time.time + duration;
    }
    public void ApplyVelocityDominant(float velocityX, float velocityY, float duration)
    {
        RequestDominantVelocityX = velocityX;
        RequestDominantVelocityY = velocityY;
        RequestXDominantEndTime = Time.time + duration;
        RequestYDominantEndTime = Time.time + duration;
    }
    public void ApplyVerticalDominantVelocityOneshot(float VelocityY)
    {
        oneshotDominantY = VelocityY;
    }
    public void CancelImpulseDominant()
    {
        RequestDominantVelocityX = null;
        RequestDominantVelocityY = null;
        RequestXDominantEndTime = -100f;
        RequestYDominantEndTime = -100f;
    }
    #endregion

    public void PhysicsUpdate(VelocityResolver velocityResolver)
    {
        if (oneshotDominantY != null)
        {
            velocityResolver.RequestDominantY(oneshotDominantY.Value);
            oneshotDominantY = null;
        }
        if (oneshotOverrideY != null)
        {
            velocityResolver.RequestOverrideY(oneshotOverrideY.Value);
            oneshotOverrideY = null;
        }

        if (RequestXOverrideEndTime > Time.time)
        {
            if (RequestOverrideVelocityX != null)
                velocityResolver.RequestOverrideX(RequestOverrideVelocityX.Value);
        }
        if (RequestYOverrideEndTime > Time.time)
        {
            if (RequestOverrideVelocityY != null)
                velocityResolver.RequestOverrideY(RequestOverrideVelocityY.Value);
        }
        if (RequestXDominantEndTime > Time.time)
        {
            if (RequestDominantVelocityX != null)
                velocityResolver.RequestDominantX(RequestDominantVelocityX.Value);
        }
        if (RequestYDominantEndTime > Time.time)
        {
            if (RequestDominantVelocityY != null)
                velocityResolver.RequestDominantY(RequestDominantVelocityY.Value);
        }
    }
}
