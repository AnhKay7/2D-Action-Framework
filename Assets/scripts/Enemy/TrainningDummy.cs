using UnityEngine;

public class TrainningDummy : Entity
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
