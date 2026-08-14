using UnityEngine;

public static class PlayerPhysicsUtility
{
    public static float CalculateLaunchVelocity(float height, float gravity)
    {
        return Mathf.Sqrt(height * -2 * gravity);
    }
}
