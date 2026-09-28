using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private HitstunReceiver hitstunReceiver;
    [SerializeField] private float moveSpeed = 5f;

    #region Runtime Variable
    private Entity Target;
    private int requestDirection = 0;
    #endregion
    private void Awake()
    {
        hitstunReceiver = GetComponent<HitstunReceiver>();
    }

    public void PhysicsUpdate(VelocityResolver VelocityResolver)
    {
        if (hitstunReceiver != null && hitstunReceiver.IsHitstunned)
        {
            VelocityResolver.SetBaseX(0f);
            return;
        }
        VelocityResolver.SetBaseX(requestDirection * moveSpeed);
        requestDirection = 0;
    }
    public void RequestMoveDirection(int direction)
    {
        requestDirection = direction;
    }
}
