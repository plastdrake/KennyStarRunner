using UnityEngine;
using System;
using System.Collections.Generic;

public class CollisionDamage : MonoBehaviour
{
    [SerializeField]
    private float damageAmount = 10f;

    [SerializeField]
    private float damageCooldown = 1f;

    public event Action<GameObject, float> OnDamageDealt;

    // Track last damage time per object
    private Dictionary<GameObject, float> lastDamageTimes = new Dictionary<GameObject, float>();

    private void TryDealDamage(Collider other)
    {
        var target = other.gameObject;
        var damageable = target.GetComponent<IDamageable>();
        if (damageable == null)
        {
            Debug.Log("No IDamageable found on: " + target.name);
            return;
        }

        float lastTime;
        lastDamageTimes.TryGetValue(target, out lastTime);

        if (Time.time - lastTime < damageCooldown)
            return;

        Debug.Log("Dealing damage to: " + target.name);
        damageable.TakeDamage(damageAmount);
        lastDamageTimes[target] = Time.time;
        OnDamageDealt?.Invoke(target, damageAmount);
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
        // Clean up to avoid memory leaks
        lastDamageTimes.Remove(other.gameObject);
    }
}
