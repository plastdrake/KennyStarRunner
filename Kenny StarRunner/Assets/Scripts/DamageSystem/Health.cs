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

    private float baseMaxHealth = 20f;

    [Header("UI Reference")]
    [SerializeField] private HealthBar healthBar; // Assign in Inspector

    private void Awake()
    {
        playerSounds = GetComponentInChildren<PlayerSounds>();
        baseMaxHealth = MaxHealth; // Store the original value
        RefreshMaxHealthFromPowerup();
    }

    public void RefreshMaxHealthFromPowerup()
    {
        Debug.Log("RefreshMaxHealthFromPowerup called");

        float extraHealth = 0f;

        if (GameManager.Instance != null && GameManager.Instance.EquippedPowerupType == Item.ItemType.Heart)
        {
            extraHealth = 10f;
        }

        MaxHealth = baseMaxHealth + extraHealth;
        Debug.Log("MaxHealth: " + MaxHealth);

        // Always heal to full when max health increases
        CurrentHealth = MaxHealth;

        // Update the HealthBar UI
        if (healthBar != null)
        {
            Debug.Log("Calling RebuildHearts on HealthBar");
            healthBar.RebuildHearts();
        }
        else
        {
            Debug.LogWarning("HealthBar reference not set on " + gameObject.name);
        }
    }

    public void TakeDamage(float amount, GameObject obj)
    {
        DamageSounds(obj);

        // Check for GalaxyHero title and halve damage if equipped
        if (GameManager.Instance != null &&
            GameManager.Instance.EquippedTitle != null &&
            GameManager.Instance.EquippedTitle.TypeOfItem == Assets.Scripts.Store.Item.ItemType.GalaxyHero)
        {
            amount *= 0.5f;
        }

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
