using UnityEngine;

public class ObstacleExplosion : MonoBehaviour
{
    [SerializeField]
    private CollisionDamage collisionDamage;

    [Header("Explosion Effect")]
    [SerializeField]
    private ParticleSystem explosionEffectPrefab;

    [SerializeField]
    private float explosionScale = 1f;

    private bool exploded = false;

    private void Awake()
    {
        if (collisionDamage == null)
            collisionDamage = GetComponent<CollisionDamage>();
    }

    private void OnEnable()
    {
        if (collisionDamage != null)
            collisionDamage.OnDamageDealt += HandleDamageDealt;
    }

    private void OnDisable()
    {
        if (collisionDamage != null)
            collisionDamage.OnDamageDealt -= HandleDamageDealt;
    }

    private void HandleDamageDealt(GameObject target, float amount)
    {
        if (exploded) return;
        exploded = true;

        if (explosionEffectPrefab != null)
        {
            // Instantiate and set the scale
            ParticleSystem explosion = Instantiate(
                explosionEffectPrefab,
                transform.position,
                transform.rotation
            );
            explosion.transform.localScale = Vector3.one * explosionScale;
            explosion.Play();
            Destroy(explosion.gameObject, explosion.main.duration);
        }

        foreach (var renderer in GetComponentsInChildren<Renderer>())
            renderer.enabled = false;
        foreach (var collider in GetComponents<Collider>())
            collider.enabled = false;

        Destroy(gameObject, 0.05f);
    }
}
