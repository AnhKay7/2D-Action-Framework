using System;
using UnityEngine;

public class TrainningDummy : MonoBehaviour
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
