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
        //Här får man utöka så att man tar in objektet eller skickar med en sträng med objektets tag så man kan avgöra vilken explosion sound det är
        if (obj.tag.Contains("Barell"))
            playerSounds.BarrelExplosionSound();
        else if (obj.tag.Contains("Mine"))
            playerSounds.MineExplosionSound();
        else if(obj.tag.Contains("Electricity"))
            playerSounds.Electricuted();
    }
}
