using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField]
    public float MaxHealth = 100f;

    public float CurrentHealth { get; private set; }

    public event Action OnDeath;

    private void Awake()
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float amount)
    {
        if (CurrentHealth <= 0f)
            return;

        CurrentHealth -= amount;
        if (CurrentHealth <= 0f)
        {
            CurrentHealth = 0f;
            OnDeath?.Invoke();
        }
    }

    public void Heal(float amount)
    {
        if (CurrentHealth <= 0f)
            return;

        CurrentHealth = Mathf.Min(CurrentHealth + amount, MaxHealth);
    }
}
