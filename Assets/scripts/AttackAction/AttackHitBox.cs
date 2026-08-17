using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    #region Setting
    [SerializeField] private int damage = 10;
    [SerializeField] private bool applyXKnockback = true;
    [SerializeField] private float velocityXKnockback = 3f;
    [SerializeField] private bool applyYKnockback = false;
    [SerializeField] private float velocityYKnockback = 0f;
    [SerializeField] private float knockbackDuration = 0.08f;
    #endregion
    private Entity owner;
    private Collider2D hitbox;
    private HashSet<Entity> Targets = new();
    public event Action HitConfirmed;
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
            HitConfirmed?.Invoke();
            MakeTargetTakeDamage(target);
            MakeTargetReceiveKnockback(target);
        }
    }
    void MakeTargetTakeDamage(Entity target)
    {
        IDamageable damageable = target.GetComponentInChildren<IDamageable>();

        if (damageable != null)
        {
            damageable.TakeDamage(damage);
        }
    }
    int GetTargetDirectionFromAttacker(Entity target)
    {
        float deltaX = target.transform.position.x - owner.transform.position.x;
        return deltaX > 0 ? 1 : -1;
    }
    void MakeTargetReceiveKnockback(Entity target)
    {
        IKnockbackable knockbackable = target.GetComponentInChildren<IKnockbackable>();

        int direction = GetTargetDirectionFromAttacker(target);
        float? finalXKnockback = (applyXKnockback == false ? null : velocityXKnockback * direction);
        float? finalYKnockback = (applyYKnockback == false ? null : velocityYKnockback);

        if (knockbackable != null)
        {
            knockbackable.ApplyKnockback(finalXKnockback, finalYKnockback, knockbackDuration);
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
