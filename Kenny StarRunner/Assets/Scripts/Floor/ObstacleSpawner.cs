using UnityEngine;

public class ObstacleSpawner : MonoBehaviour
{
    [Tooltip("Reference to the obstacle spawn config asset.")]
    public ObstacleSpawnConfig spawnConfig;

    void Start()
    {
        if (spawnConfig != null)
        {
            SpawnObstacle();
        }
    }

    void SpawnObstacle()
    {
        var obstaclePrefabs = spawnConfig.obstaclePrefabs;
        float totalChance = 0f;
        foreach (var entry in obstaclePrefabs)
        {
            totalChance += entry.spawnChance;
        }

        if (totalChance <= 0f)
            return; // No valid chances set

        // Generate a random value between 0 and 100
        float randomValue = Random.Range(0f, 100f);

        // If randomValue is greater than totalChance, spawn nothing
        if (randomValue > totalChance)
            return;

        // Otherwise, select which prefab to spawn
        float cumulative = 0f;
        foreach (var entry in obstaclePrefabs)
        {
            cumulative += entry.spawnChance;
            if (randomValue <= cumulative)
            {
                if (entry.prefab != null)
                {
                    Instantiate(entry.prefab, transform.position, transform.rotation, transform);
                }
                break;
            }
        }
    }
}
