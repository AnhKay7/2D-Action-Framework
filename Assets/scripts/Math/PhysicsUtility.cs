using UnityEngine;

public static class PhysicsUtility
{
    public static float CalculateLaunchVelocity(float height, float gravity)
    {
        return Mathf.Sqrt(height * -2 * gravity);
    }

    public static float Distance(Entity a, Entity b)
    {
        return Vector2.Distance(a.transform.position, b.transform.position);
    }

    public static float HorizontalDistance(Entity a, Entity b)
    {
        return Mathf.Abs(
            a.transform.position.x -
            b.transform.position.x
        );
    }
}
