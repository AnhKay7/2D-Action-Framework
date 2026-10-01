using UnityEngine;

public class Entity : MonoBehaviour
{
    public enum Faction
    {
        Ally,
        Neutral,
        Hostile
    }

    [SerializeField] private Faction faction = Faction.Neutral;

    public Faction EntityFaction => faction;
    public virtual bool CanReceiveHit => true;
    public virtual bool IsAlive => true;
    public virtual bool CanBeInterrupted => true;
    public int FacingDirection { get; protected set; } = 1;
    public virtual bool IsFacingRight => FacingDirection > 0;

    public virtual void SetFacingDirection(int direction)
    {
        if (direction == 0)
            return;

        FacingDirection = direction > 0 ? 1 : -1;
    }
}
