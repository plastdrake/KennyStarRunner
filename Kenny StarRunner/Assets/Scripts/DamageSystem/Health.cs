using UnityEngine;
using System;
using Assets.Scripts.Store; // For Item.ItemType

public class Health : MonoBehaviour, IDamageable
{
    [SerializeField]
    public float MaxHealth = 20f;

    public float CurrentHealth { get; private set; }

    public event Action OnDeath;
    private PlayerSounds playerSounds;

    private void Awake()
    {
        playerSounds = GetComponentInChildren<PlayerSounds>();

        float extraHealth = 0f;

        // Check if the player has a heart powerup equipped
        if (GameManager.Instance != null && GameManager.Instance.EquippedPowerupType == Item.ItemType.Heart)
        {
            // If you want to use a fixed value, set it here (e.g., 5f)
            // extraHealth = 5f;

            // If you want to use the Powerup asset's Amount, you need to store a reference to the equipped Powerup in GameManager
            // Example (if you add EquippedPowerup to GameManager):
            // if (GameManager.Instance.EquippedPowerup != null)
            //     extraHealth = GameManager.Instance.EquippedPowerup.Amount;

            extraHealth = 5f; // Change this value as needed or fetch from Powerup asset
        }

        MaxHealth += extraHealth;
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(float amount, GameObject obj)
    {
        DamageSounds(obj);
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

    private void DamageSounds(GameObject obj)
    {
        if (obj.tag.Contains("Barell"))
            playerSounds.BarrelExplosionSound();
        else if (obj.tag.Contains("Mine"))
            playerSounds.MineExplosionSound();
        else if (obj.tag.Contains("Electricity"))
            playerSounds.Electricuted();
    }
}
