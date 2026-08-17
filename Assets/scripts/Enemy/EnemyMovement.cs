using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5f;
    private Entity Target;
    public void SetTarget(Entity Target)
    {
        this.Target = Target;
    }
    private void ChaseTarget(VelocityResolver VelocityResolver)
    {

        if (Target == null)
        {
            VelocityResolver.SetBaseX(0f);
            return;
        }
        float deltaX = Target.transform.position.x - transform.position.x;
        int direction = deltaX > 0 ? 1 : -1;

        VelocityResolver.SetBaseX(direction * moveSpeed);
        //Debug.Log(Target.EntityFaction);
    }

    public void PhysicsUpdate(VelocityResolver VelocityResolver)
    {
        ChaseTarget(VelocityResolver);
    }
}
