using UnityEngine;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Animator animator;
    [SerializeField] private MonoBehaviour movementScript; // Reference to your movement script
    [SerializeField] private Rigidbody rb; // Reference to Rigidbody
    [SerializeField] private Collider deathCollider; // Reference to CapsuleCollider (or similar)

    private void Awake()
    {
        if (health == null)
            health = GetComponent<Health>();
        if (animator == null)
            animator = GetComponent<Animator>();
        if (rb == null)
            rb = GetComponent<Rigidbody>();
        if (deathCollider == null)
            deathCollider = GetComponent<Collider>();
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
        // Play the fall down animation
        if (animator != null)
            animator.SetTrigger("FallDown");

        // Disable movement
        if (movementScript != null)
            movementScript.enabled = false;

        // If using CharacterController, disable it and enable Rigidbody
        var characterController = GetComponent<CharacterController>();
        if (characterController != null)
            characterController.enabled = false;

        // Enable the collider for physics
        if (deathCollider != null)
            deathCollider.enabled = true;

        if (rb != null)
        {
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.linearVelocity = Vector3.zero; // Correct property is 'velocity'
        }
    }
}
