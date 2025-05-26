using UnityEngine;

public class FloatingBehavior : MonoBehaviour
{
    [Header("Floating Settings")]
    [Tooltip("How high the object bobs up and down.")]
    [SerializeField] private float amplitude = 0.5f;

    [Tooltip("How fast the object bobs up and down.")]
    [SerializeField] private float frequency = 1f;

    private Vector3 startPosition;

    void Start()
    {
        startPosition = transform.position;
    }

    void Update()
    {
        float newY = startPosition.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(startPosition.x, newY, startPosition.z);
    }
}