using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskHealthUI : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Image firelifePrefab;
    [SerializeField] private Transform firelifeContainer;
    [SerializeField] private Sprite fullFirelifeSprite;
    [SerializeField] private Sprite emptyFirelifeSprite;

    private readonly List<Image> lives = new List<Image>();

    private void OnEnable()
    {
        if (health != null)
            health.OnHealthChanged += HandleHealthChanged;
    }
    private void OnDisable()
    {
        if (health != null)
            health.OnHealthChanged -= HandleHealthChanged;
    }
    private void Start()
    {
        if (health == null)
            return;
        HandleHealthChanged(health.CurrentHealth, health.MaxHealth);
    }
    private void HandleHealthChanged(float current, float max)
    {
        int firelivesCount = Mathf.CeilToInt(max);
        if (lives.Count != firelivesCount)
            BuildMasks(firelivesCount);
        for (int i = 0; i < lives.Count; i++)
            lives[i].sprite = i < current ? fullFirelifeSprite : emptyFirelifeSprite;
    }
    private void BuildMasks(int count)
    {
        foreach (Image firelife in lives)
            Destroy(firelife.gameObject);
        lives.Clear();
        for (int i = 0; i < count; i++)
            lives.Add(Instantiate(firelifePrefab, firelifeContainer));
    }
}