using UnityEngine;

public class KnockbackReceiver : MonoBehaviour, IKnockbackable
{
    private ImpulseController impulseController;

    public void Initialize(ImpulseController impulseController)
    {
        this.impulseController = impulseController;
    }
    public void ApplyKnockback(float? velocityX, float? velocityY, float duration)
    {
        if (velocityX != null)
            impulseController.ApplyHorizontalDominantVelocity(velocityX.Value, duration);
        if (velocityY != null)
            impulseController.ApplyVerticalDominantVelocity(velocityY.Value, duration);
    }
}
