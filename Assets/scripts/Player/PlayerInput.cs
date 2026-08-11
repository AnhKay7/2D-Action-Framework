using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public bool JumpHeld { get; private set; }
    public bool JumpReleased { get; private set; }
    public float MoveDirection { get; private set; }
    public bool IsFacingRight { get; private set; } = true;
    public int AttackVerticalDirection { get; private set; } = 0;

    [Header("Input Buffer Settings")]
    [SerializeField] private float jumpBufferTime = 0.15f;
    [SerializeField] private float dashBufferTime = 0.15f;
    [SerializeField] private float attackBufferTime = 0.1f;

    private float lastTimePressedJump = -100f;
    private float lastTimePressedDash = -100f;
    private float lastTimePressedAttack = -100f;

    public bool JumpInput => Time.time < lastTimePressedJump + jumpBufferTime;
    public bool DashInput => Time.time < lastTimePressedDash + dashBufferTime;
    public bool AttackInput => Time.time < lastTimePressedAttack + attackBufferTime;
    public void GetPlayerInput()
    {

        if (Input.GetKeyDown(KeyCode.Z))
            lastTimePressedJump = Time.time;
        JumpHeld = Input.GetKey(KeyCode.Z);
        JumpReleased = Input.GetKeyUp(KeyCode.Z);

        if (Input.GetKeyDown(KeyCode.C))
            lastTimePressedDash = Time.time;

        if (Input.GetKeyDown(KeyCode.X))
        {
            lastTimePressedAttack = Time.time;
            AttackVerticalDirection = GetPlayerAttackVerticalDirection();
        }

        MoveDirection = GetPlayerMoveDirection();
        if (MoveDirection != 0f)
        {
            IsFacingRight = MoveDirection > 0f;
        }
    }
    private float GetPlayerMoveDirection()
    {
        float rightValue = Input.GetKey(KeyCode.RightArrow) ? 1f : 0f;
        float leftValue = Input.GetKey(KeyCode.LeftArrow) ? 1f : 0f;

        return rightValue - leftValue;
    }
    private int GetPlayerAttackVerticalDirection()
    {
        int upValue = Input.GetKey(KeyCode.UpArrow) ? 1 : 0;
        int downValue = Input.GetKey(KeyCode.DownArrow) ? 1 : 0;

        return upValue - downValue;
    }
    public void UseJumpInput() => lastTimePressedJump = -100f;
    public void UseDashInput() => lastTimePressedDash = -100f;
    public void UseAttackInput() => lastTimePressedAttack = -100f;
}
