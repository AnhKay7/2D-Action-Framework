using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    private Entity owner;
    private Collider2D hitbox;
    private HashSet<Entity> Targets = new();
    public event Action<Entity> TargetDetected;
    private void Awake()
    {
        hitbox = GetComponent<Collider2D>();
        hitbox.enabled = false;
        owner = GetComponentInParent<Entity>();
    }
    public void Activate()
    {
        Targets.Clear();
        hitbox.enabled = true;
    }
    public void Deactivate()
    {
        hitbox.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Entity target = collision.GetComponentInParent<Entity>();

        if (target == null)
            return;

        if (target.EntityFaction == owner.EntityFaction)
            return;

        if (Targets.Add(target)){
            TargetDetected?.Invoke(target);
        }
    }
    private void OnDrawGizmos()
    {
        if (hitbox == null)
            return;

        Gizmos.DrawWireCube(
            hitbox.bounds.center,
            hitbox.bounds.size
        );
    }
}
