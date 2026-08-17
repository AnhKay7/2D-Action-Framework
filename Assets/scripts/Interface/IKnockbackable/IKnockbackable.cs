using UnityEngine;

public interface IKnockbackable
{
    void ApplyKnockback(float? velocityX, float? velocityY, float duration);
}
