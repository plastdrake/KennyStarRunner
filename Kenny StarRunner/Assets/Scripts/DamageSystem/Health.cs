using UnityEngine;
using System;

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField]
    public float MaxHealth = 100f;

    public float CurrentHealth { get; private set; }

    public event Action OnDeath;
    private PlayerSounds playerSounds;

    private void Awake()
    {
        playerSounds = GetComponentInChildren<PlayerSounds>();
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float amount)
    {
        DamageSounds();
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

    private void DamageSounds()
    {
        //Här får man utöka så att man tar in objektet eller skickar med en sträng med objektets tag så man kan avgöra vilken explosion sound det är
        if (tag.Contains("Barell"))
            playerSounds.BarrelExplosionSound();
        else if (tag.Contains("Mine"))
            playerSounds.MineExplosionSound();
    }
}
