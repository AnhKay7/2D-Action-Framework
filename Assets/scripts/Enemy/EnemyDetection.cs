using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private Entity owner;
    private HashSet<Entity> targetsInRange = new();
    public Entity Target { get; private set; }
    private void Awake()
    {
        owner = GetComponentInParent<Entity>();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Entity thisTarget = other.GetComponentInParent<Entity>();
        if (thisTarget == null)
            return;
        if (thisTarget.EntityFaction == owner.EntityFaction)
            return;
        if (targetsInRange.Add(thisTarget))
        {
            if (Target == null)
                Target = thisTarget;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Entity thisTarget = other.GetComponentInParent<Entity>();
        targetsInRange.Remove(thisTarget);

        if (Target != thisTarget)
            return;

        Target = targetsInRange.FirstOrDefault();
    }
}
