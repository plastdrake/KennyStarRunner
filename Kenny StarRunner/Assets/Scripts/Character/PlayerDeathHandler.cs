using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class PlayerDeathHandler : MonoBehaviour
{
    [SerializeField] private Health health;
    [SerializeField] private Animator animator;
    [SerializeField] private MonoBehaviour movementScript; // Reference to your movement script
    [SerializeField] private Rigidbody rb; // Reference to Rigidbody
    [SerializeField] private Collider deathCollider; // Reference to CapsuleCollider (or similar)
    [SerializeField] private PlayerLifetimeCounter lifetimeCounter; // Reference to timer script
    private PlayerSounds playerSounds;

    [SerializeField] private float fadeDuration = 1f; // Duration of fade

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
        if (lifetimeCounter == null)
            lifetimeCounter = GetComponent<PlayerLifetimeCounter>();
        playerSounds = GetComponentInChildren<PlayerSounds>();
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

        // Play death sound
        if (playerSounds != null)
            playerSounds.DeadSound();

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

        // Stop the lifetime counter
        if (lifetimeCounter != null)
            lifetimeCounter.OnPlayerDeath();

        // Start coroutine to fade and load store scene after delay
        StartCoroutine(FadeAndLoadLobbyAfterDelay(3f));
    }

    private IEnumerator FadeAndLoadLobbyAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Find the fade background by tag
        GameObject fadeObj = GameObject.FindGameObjectWithTag("FadeBG");
        CanvasGroup fadeGroup = null;
        if (fadeObj != null)
            fadeGroup = fadeObj.GetComponent<CanvasGroup>();

        // Fade to black
        if (fadeGroup != null)
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.unscaledDeltaTime;
                fadeGroup.alpha = Mathf.Clamp01(t / fadeDuration);
                yield return null;
            }
            fadeGroup.alpha = 1f;
        }

        SceneManager.LoadScene("StoreScene");
    }
}
