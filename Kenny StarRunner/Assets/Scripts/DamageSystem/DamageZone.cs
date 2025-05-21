using UnityEngine;
using System;
using System.Collections.Generic;

public class DamageZone : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 10f;

    [SerializeField]
    private float damageCooldown = 1f;

    public event Action<GameObject, float> OnDamageDealt;

    // Tracks last damage time per object
    private Dictionary<GameObject, float> lastDamageTimes = new Dictionary<GameObject, float>();

    private void TryDealDamage(Collider other)
    {
        var damageable = other.GetComponent<IDamageable>();
        if (damageable == null)
            return;

        float lastTime;
        lastDamageTimes.TryGetValue(other.gameObject, out lastTime);

        if (Time.time - lastTime < damageCooldown)
            return;

        damageable.TakeDamage(damageAmount);
        lastDamageTimes[other.gameObject] = Time.time;
        OnDamageDealt?.Invoke(other.gameObject, damageAmount);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryDealDamage(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TryDealDamage(other);
    }

    private void OnTriggerExit(Collider other)
    {
        // Optional: Clean up dictionary to avoid memory leaks
        lastDamageTimes.Remove(other.gameObject);
    }
}
