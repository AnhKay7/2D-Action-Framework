public class AttackEffectProcessor
{
    public bool ApplyAttackEffectToTarget(Entity attacker, Entity target, AttackData attackData, int attackDirection)
    {
        HitReceiver hitReceiver = target.GetComponentInChildren<HitReceiver>();

        return hitReceiver?.ReceiveHit(attacker, attackData, attackDirection) ?? false;
    }
}
