using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Animator animator;
    [SerializeField] private MonoBehaviour movementScript; // Reference to your movement script

    private void Awake()
    {
        if (health == null)
            health = GetComponent<Health>();
        if (animator == null)
            animator = GetComponent<Animator>();
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

    private void HandleDeath()
    {
        // Play the fall down animation (replace "FallDown" with your animation state name)
        if (animator != null)
            animator.SetTrigger("FallDown");

        // Disable movement
        if (movementScript != null)
            movementScript.enabled = false;
    }
}
