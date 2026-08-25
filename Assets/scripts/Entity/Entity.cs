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
}
