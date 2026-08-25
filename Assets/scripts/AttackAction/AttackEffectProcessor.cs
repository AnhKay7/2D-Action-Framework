public class AttackEffectProcessor
{
    public bool ApplyAttackEffectToTarget(Entity attacker, Entity target, AttackData attackData, int horizontalAttackDirection)
    {
        HitReceiver hitReceiver = target.GetComponentInChildren<HitReceiver>();

        return hitReceiver == null ? false : hitReceiver.ReceiveHit(attacker, attackData, horizontalAttackDirection);
    }
}
