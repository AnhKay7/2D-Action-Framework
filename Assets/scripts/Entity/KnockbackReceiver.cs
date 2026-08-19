using UnityEngine;

public class KnockbackReceiver : MonoBehaviour, IKnockbackable
{
    private ImpulseController impulseController;

    public void Initialize(ImpulseController impulseController)
    {
        this.impulseController = impulseController;
    }
    public void ApplyKnockback(float? velocityX, float? velocityY, float durationX, float durationY)
    {
        if (velocityX != null)
            impulseController.ApplyHorizontalDominantVelocity(velocityX.Value, durationX);
        if (velocityY != null)
            impulseController.ApplyVerticalDominantVelocity(velocityY.Value, durationY);
    }
}
