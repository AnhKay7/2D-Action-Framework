using System.Collections.Generic;
using UnityEngine;

public class AttackHitBox : MonoBehaviour
{
    [SerializeField] private int damage = 10;

    private Collider2D hitbox;
    private HashSet <IDamageable> damagedTargets = new();
    private void Awake()
    {
        hitbox = GetComponent<Collider2D>();
        hitbox.enabled = false;
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
        if (collision.TryGetComponent<IDamageable>(out var damageable))
        {
            if (damagedTargets.Add(damageable))
            {
                damageable.TakeDamage(damage);
            }
        }
    }
}
