using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class PlayerImpulseController : MonoBehaviour
{
    private VelocityResolver velocityResolver;
    private float RequestVelocityX;
    private float RequestEndTime;
    public void Initialize(VelocityResolver VelocityResolver)
    {
        velocityResolver = VelocityResolver;
    }
    public void ApplyHorizontalVelocity(float VelocityX, float duration)
    {
        RequestVelocityX = VelocityX;
        RequestEndTime = Time.time + duration;
    }
    public void ApplyVerticalVelocity(float VelocityY)
    {
        velocityResolver.RequestOverrideY(VelocityY);
    }
    public void PhysicsUpdate()
    {
        if (RequestEndTime > Time.time)
            velocityResolver.RequestOverrideX(RequestVelocityX);
    }
}
