using System;
using UnityEngine;

public class HitReceiver : MonoBehaviour
{
    private Entity owner;
    public event Action<Entity, int> HitReceived;
    private void Awake()
    {
        owner = GetComponentInParent<Entity>();
    }
    public bool ReceiveHit(Entity attacker, AttackData attackData, int attackDirection)
    {
        if (owner.CanReceiveHit)
        {
            ApplyAttackEffect(attacker, attackData, attackDirection);
            HitReceived?.Invoke(attacker, attackDirection);
            return true;
        }
        return false;
    }
    private void ApplyAttackEffect(Entity attacker, AttackData attackData, int attackDirection)
    {
        ApplyDamage(attackData);
        ApplyKnockback(attackData, attackDirection);
        ApplyHitstun(attackData);
        ApplyHitReaction();
    }
    private void ApplyDamage(AttackData attackData)
    {
        IDamageable damageable = owner.GetComponentInChildren<IDamageable>();

        damageable?.TakeDamage(attackData.Damage);
    }
    private void ApplyKnockback(AttackData attackData, int attackDirection)
    {
        IKnockbackable knockbackable = owner.GetComponentInChildren<IKnockbackable>();

        float? finalXKnockback = (attackData.ApplyXKnockback == false ? null : attackData.KnockbackX * attackDirection);
        float? finalYKnockback = (attackData.ApplyYKnockback == false ? null : attackData.KnockbackY);

        knockbackable?.ApplyKnockback(finalXKnockback, finalYKnockback, attackData.KnockbackXDuration, attackData.KnockbackYDuration);
    }
    private void ApplyHitReaction()
    {
        HitReactionController reaction = owner.GetComponentInChildren<HitReactionController>();

        reaction?.ReactionToHit();
    }
    private void ApplyHitstun(AttackData attackData)
    {
        IHitstunnable hitstunnable = owner.GetComponentInChildren<IHitstunnable>();

        hitstunnable?.ApplyHitstun(attackData.HitstunDuration);
    }
}
