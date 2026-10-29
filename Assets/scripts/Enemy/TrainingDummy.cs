using UnityEngine;

public class TrainingDummy : Entity
{
    [SerializeField] private Health health;
    private void Awake()
    {
        health.OnDeath += Die;
    }
    private void Die()
    {
        Destroy(gameObject);
    }
}
