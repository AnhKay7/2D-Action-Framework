public class AttackEffectProcessor
{
    public void ApplyAttackEffect(Entity attacker, Entity target, AttackData attackData)
    {
        ApplyDamageToTarget(target, attackData);
        ApplyKnockbackToTarget(attacker, target, attackData);
    }
    private void ApplyDamageToTarget(Entity target, AttackData attackData)
    {
        IDamageable damageable = target.GetComponentInChildren<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(attackData.Damage);
        }
    }
    private void ApplyKnockbackToTarget(Entity attacker, Entity target, AttackData attackData)
    {
        IKnockbackable knockbackable = target.GetComponentInChildren<IKnockbackable>();

        int direction = GetTargetDirectionFromAttacker(attacker, target);
        float? finalXKnockback = (attackData.ApplyXKnockback == false ? null : attackData.KnockbackX * direction);
        float? finalYKnockback = (attackData.ApplyYKnockback == false ? null : attackData.KnockbackY);

        if (knockbackable != null)
        {
            knockbackable.ApplyKnockback(finalXKnockback, finalYKnockback, attackData.KnockbackDuration);
        }
    }
    private int GetTargetDirectionFromAttacker(Entity attacker, Entity target)
    {
        float deltaX = target.transform.position.x - attacker.transform.position.x;
        return deltaX > 0 ? 1 : -1;
    }
}
