using System;
using UnityEngine;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100;
    private float currentHealth;
    public bool IsDead => currentHealth <= 0;
    public event Action OnDeath;
    private void Awake()
    {
        currentHealth = maxHealth;
    }
    public void TakeDamage(int damage)
    {
        if (IsDead)
            return;
        currentHealth -= damage;
        if (currentHealth <= 0)
            OnDeath?.Invoke();
    }
}
