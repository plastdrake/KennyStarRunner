using UnityEngine;

public class GrowAtSpawn : MonoBehaviour
{
    [SerializeField] private float growDuration = 0.5f; // Duration in seconds
    [SerializeField] private Transform meshTransform;    // Assign in Inspector or via code

    private Vector3 targetScale;
    private float timer = 0f;

    void Start()
    {
        if (meshTransform == null)
        {
            // Try to auto-find the first MeshRenderer in children if not set
            var meshRenderer = GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
                meshTransform = meshRenderer.transform;
            else
                Debug.LogWarning("GrowAtSpawn: No meshTransform assigned and no MeshRenderer found in children.");
        }

        if (meshTransform != null)
        {
            targetScale = meshTransform.localScale;
            meshTransform.localScale = Vector3.zero; // Start at scale (0,0,0)
        }
    }

    void Update()
    {
        if (meshTransform == null)
            return;

        if (timer < growDuration)
        {
            timer += Time.deltaTime;
            float t = Mathf.Clamp01(timer / growDuration);
            meshTransform.localScale = Vector3.Lerp(Vector3.zero, targetScale, t);
        }
        else
        {
            // Ensure final scale is set
            meshTransform.localScale = targetScale;
            enabled = false; // Disable script after growth
        }
    }
}
