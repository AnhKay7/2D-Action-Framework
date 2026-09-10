using System.Collections.Generic;
using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    private Entity owner;
    private HashSet<Entity> entitiesInRange = new();
    public Entity Target { get; private set; }
    private void Awake()
    {
        owner = GetComponentInParent<Entity>();
    }
    private bool IsValidTarget(Entity target)
    {
        return target != null && target.IsAlive == true;
    }
    private Entity FindNewTarget()
    {
        foreach (Entity target in entitiesInRange)
        {
            if (!IsValidTarget(target))
                continue;

            return target;
        }
        return null;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        Entity thisTarget = other.GetComponentInParent<Entity>();
        if (thisTarget == null)
            return;
        if (thisTarget.EntityFaction == owner.EntityFaction)
            return;
        if (entitiesInRange.Add(thisTarget))
        {
            if (Target == null && IsValidTarget(thisTarget))
                Target = thisTarget;
        }
    }
    private void Update()
    {
        if (IsValidTarget(Target))
            return;

        Target = FindNewTarget();
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        Entity thisTarget = other.GetComponentInParent<Entity>();
        entitiesInRange.Remove(thisTarget);

        if (Target != thisTarget)
            return;

        Target = FindNewTarget();
    }
}
