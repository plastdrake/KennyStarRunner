using System.Collections.Generic;
using UnityEngine;

public class OrbitScript : MonoBehaviour
{
    [Header("Orbit Prefabs & Counts")]
    public List<GameObject> orbitPrefabs; // List of prefabs to orbit
    public List<int> prefabCounts; // Number of each prefab to spawn

    [Header("Orbit Parameters")]
    public float orbitRadius = 5f; // Base radius of the orbit
    public float orbitSpacing = 1.5f; // Minimum distance between objects
    public float orbitDensity = 1f; // Multiplier for how tightly packed objects are
    public float orbitSpeed = 30f; // Degrees per second for orbiting
    [Tooltip("Maximum random scale added/subtracted to each object's orbit radius.")]
    public float radiusRandomness = 1.0f;

    [Header("Height Variation")]
    [Tooltip("Min/Max Y offset for each object's orbit position.")]
    public Vector2 heightRange = new Vector2(-1f, 1f);

    [Header("Scale Randomness")]
    public Vector2 scaleRange = new Vector2(0.8f, 1.2f); // Min/Max random scale

    [Header("Self Rotation")]
    public float selfRotationSpeedMin = 30f;
    public float selfRotationSpeedMax = 90f;

    [Header("Collision")]
    [Tooltip("If true, removes all Collider and Rigidbody components from instantiated objects and their children.")]
    public bool removeColliders = false;

    private class OrbitObject
    {
        public GameObject instance;
        public float angleOffset;
        public float selfRotationSpeed;
        public Vector3 axis;
        public float radius;
        public float heightOffset;
    }

    private List<OrbitObject> orbitObjects = new List<OrbitObject>();

    void Start()
    {
        if (orbitPrefabs.Count != prefabCounts.Count)
        {
            Debug.LogError("orbitPrefabs and prefabCounts must have the same length.");
            return;
        }

        // Calculate total number of objects
        int totalObjects = 0;
        foreach (int count in prefabCounts)
            totalObjects += count;

        float angleStep = 360f / (totalObjects * orbitDensity);

        int objIndex = 0;
        for (int i = 0; i < orbitPrefabs.Count; i++)
        {
            for (int j = 0; j < prefabCounts[i]; j++)
            {
                float angle = objIndex * angleStep + Random.Range(-angleStep * 0.2f, angleStep * 0.2f);
                float radius = orbitRadius + Random.Range(-orbitSpacing, orbitSpacing) + Random.Range(-radiusRandomness, radiusRandomness);
                float heightOffset = Random.Range(heightRange.x, heightRange.y);

                GameObject obj = Instantiate(orbitPrefabs[i], transform);
                float scale = Random.Range(scaleRange.x, scaleRange.y);
                obj.transform.localScale = Vector3.one * scale;

                if (removeColliders)
                {
                    // Remove all Collider components (including inactive children)
                    foreach (var collider in obj.GetComponentsInChildren<Collider>(true))
                    {
                        Destroy(collider);
                    }
                    // Optionally remove all Rigidbody components as well
                    foreach (var rb in obj.GetComponentsInChildren<Rigidbody>(true))
                    {
                        Destroy(rb);
                    }
                }

                var orbitObj = new OrbitObject
                {
                    instance = obj,
                    angleOffset = angle,
                    selfRotationSpeed = Random.Range(selfRotationSpeedMin, selfRotationSpeedMax),
                    axis = Random.onUnitSphere, // Random axis for self-rotation
                    radius = radius,
                    heightOffset = heightOffset
                };
                orbitObjects.Add(orbitObj);
                objIndex++;
            }
        }
    }

    void Update()
    {
        float time = Time.time;
        for (int i = 0; i < orbitObjects.Count; i++)
        {
            OrbitObject orbitObj = orbitObjects[i];
            float angle = orbitObj.angleOffset + orbitSpeed * time;
            float rad = angle * Mathf.Deg2Rad;
            Vector3 pos = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * orbitObj.radius;
            pos.y = orbitObj.heightOffset;
            orbitObj.instance.transform.localPosition = pos;

            // Self-rotation
            orbitObj.instance.transform.Rotate(orbitObj.axis, orbitObj.selfRotationSpeed * Time.deltaTime, Space.Self);
        }
    }
}
