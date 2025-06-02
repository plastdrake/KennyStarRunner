using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Health health;

    [SerializeField]
    private GameObject heartPrefab; // Assign your HeartImage prefab here

    [SerializeField]
    private Transform heartsContainer; // Assign your HeartsContainer here

    private List<Image> hearts = new List<Image>();
    private int heartsCount;

    private void Awake()
    {
        if (health == null)
            health = GetComponentInParent<Health>();
    }

    private void Start()
    {
        RebuildHearts();
    }

    private void Update()
    {
        UpdateHearts();
    }

    public void RebuildHearts()
    {
        Debug.Log($"[HealthBar] RebuildHearts called on {gameObject.name}. MaxHealth: {health.MaxHealth}, CurrentHealth: {health.CurrentHealth}");

        // Remove old hearts
        foreach (var heart in hearts)
        {
            if (heart != null)
            {
                Debug.Log("[HealthBar] Destroying old heart");
                Destroy(heart.gameObject);
            }
        }
        hearts.Clear();

        // Create new hearts based on current MaxHealth
        if (health != null && heartPrefab != null && heartsContainer != null)
        {
            heartsCount = Mathf.CeilToInt(health.MaxHealth / 10f);
            Debug.Log($"[HealthBar] Hearts to create: {heartsCount}");

            for (int i = 0; i < heartsCount; i++)
            {
                GameObject heartObj = Instantiate(heartPrefab, heartsContainer);
                Image heartImage = heartObj.GetComponent<Image>();
                hearts.Add(heartImage);
                Debug.Log($"[HealthBar] Created heart {i + 1}/{heartsCount}");
            }
            Debug.Log($"[HealthBar] Total hearts in list: {hearts.Count}");
            UpdateHearts();
        }
        else
        {
            Debug.LogWarning($"[HealthBar] Setup incomplete: health={health}, heartPrefab={heartPrefab}, heartsContainer={heartsContainer}");
        }
    }

    private void UpdateHearts()
    {
        if (health == null) return;

        int fullHearts = Mathf.FloorToInt(health.CurrentHealth / 10f);

        for (int i = 0; i < heartsCount; i++)
        {
            Image heartImage = hearts[i];
            heartImage.enabled = true;
            heartImage.color = (i < fullHearts) ? Color.white : new Color(1f, 1f, 1f, 0f); // Invisible empty hearts
        }
    }

    // Optionally keep this if you want to hide the health bar on death
    private void OnEnable()
    {
        if (health != null)
            health.OnDeath += HandleDeath;
    }

    private void OnDisable()
    {
        if (health != null)
            health.OnDeath -= HandleDeath;
    }

    private void HandleDeath()
    {
        gameObject.SetActive(false);
    }
}
