using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    [SerializeField] private int damage = 10;
    private Entity owner;
    private Collider2D hitbox;
    private HashSet <IDamageable> damagedTargets = new();
    public event Action HitConfirmed;
    private void Awake()
    {
        hitbox = GetComponent<Collider2D>();
        hitbox.enabled = false;
        owner = GetComponentInParent<Entity>();
    }
    public void Activate()
    {
        damagedTargets.Clear();
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

        IDamageable damageable = collision.GetComponentInParent<IDamageable>();

        if (damageable != null && damagedTargets.Add(damageable))
        {
            HitConfirmed?.Invoke();
            damageable.TakeDamage(damage);
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
