using System;
using UnityEngine;

public class HitReceiver : MonoBehaviour
{
    private Entity owner;
    private void Awake()
    {
        owner = GetComponentInParent<Entity>();
    }
    public bool ReceiveHit(Entity attacker, AttackData attackData, int horizontalAttackDirection)
    {
        if (owner.CanReceiveHit)
        {
            ApplyAttackEffect(attacker, attackData, horizontalAttackDirection);
            return true;
        }
        return false;
    }
    private void ApplyAttackEffect(Entity attacker, AttackData attackData, int attackHorizontalDirection)
    {
        ApplyDamage(attackData);
        ApplyKnockback(attackData, attackHorizontalDirection);
        AppHitstun(attackData);
        ApplyHitReaction();
    }
    private void ApplyDamage(AttackData attackData)
    {
        IDamageable damageable = owner.GetComponentInChildren<IDamageable>();

        damageable?.TakeDamage(attackData.Damage);
    }
    private void ApplyKnockback(AttackData attackData, int horizontalAttackDirection)
    {
        IKnockbackable knockbackable = owner.GetComponentInChildren<IKnockbackable>();

        float? finalXKnockback = (attackData.ApplyXKnockback == false ? null : attackData.KnockbackX * horizontalAttackDirection);
        float? finalYKnockback = (attackData.ApplyYKnockback == false ? null : attackData.KnockbackY);

        knockbackable?.ApplyKnockback(finalXKnockback, finalYKnockback, attackData.KnockbackXDuration, attackData.KnockbackYDuration);
    }
    private void ApplyHitReaction()
    {
        HitReactionController reaction = owner.GetComponentInChildren<HitReactionController>();

        reaction?.ReactionToHit();
    }
    private void AppHitstun(AttackData attackData)
    {
        IHitstunnable hitstunnable = owner.GetComponentInChildren<IHitstunnable>();

        hitstunnable?.ApplyHitstun(attackData.HitstunDuration);
    }
}
