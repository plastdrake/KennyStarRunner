using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField]
    private Health health; // Reference to the Health component

    [SerializeField]
    private Slider slider; // Reference to the UI Slider

    private void Awake()
    {
        if (health == null)
            health = GetComponentInParent<Health>();

        if (slider == null)
            slider = GetComponentInChildren<Slider>();
    }

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

    private void Start()
    {
        if (health != null && slider != null)
        {
            slider.maxValue = health.MaxHealth;
            slider.value = health.CurrentHealth;
        }
    }

    private void Update()
    {
        if (health != null && slider != null)
        {
            slider.value = health.CurrentHealth;
        }
    }

    private void HandleDeath()
    {
        // Optionally hide or disable the health bar on death
        gameObject.SetActive(false);
    }
}
