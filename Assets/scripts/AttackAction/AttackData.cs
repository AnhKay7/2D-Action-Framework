using UnityEngine;

[System.Serializable]
public class AttackData
{
    [SerializeField] private int damage;
    public int Damage => damage;
    [SerializeField] private bool applyXKnockback;
    public bool ApplyXKnockback => applyXKnockback;
    [SerializeField] private float knockbackX;
    public float KnockbackX => knockbackX;
    [SerializeField] private bool applyYKnockback;
    public bool ApplyYKnockback => applyYKnockback;
    [SerializeField] private float knockbackY;
    public float KnockbackY => knockbackY;

    [SerializeField] private float knockbackXDuration;
    public float KnockbackXDuration => knockbackXDuration;

    [SerializeField] private float knockbackYDuration;
    public float KnockbackYDuration => knockbackYDuration;

    [SerializeField] private AttackHitBox hitBox;
    public AttackHitBox HitBox => hitBox;
    [SerializeField] private float windupTime;
    public float WindupTime => windupTime;
    [SerializeField] private float activeTime;
    public float ActiveTime => activeTime;
    [SerializeField] private float recoveryTime;
    public float RecoveryTime => recoveryTime;
}
