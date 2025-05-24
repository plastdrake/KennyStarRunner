using UnityEngine;

[System.Serializable]
public class ObstaclePrefabChance
{
    public GameObject prefab;
    [Range(0, 100)]
    public float spawnChance; // Percentage chance (0-100)
}
