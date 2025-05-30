using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    public GameObject straightForward;
    public List<GameObject> straightForwardWithHoles;
    public List<GameObject> slideUnderObstacles;
    public List<GameObject> rampUpPrefabs;
    public List<GameObject> rampDownPrefabs;
    public GameObject turnLeft;
    public GameObject turnRight;
    public GameObject startObject;
    public GameObject player;
    //private HashSet<Vector3Int> occupiedPositions = new HashSet<Vector3Int>();

    private Vector3 currentPosition;
    private float currentYAngle;
    private bool lastWasCorner = false;
    private int straightStreakCount = 0;
    private float gapDistance = 8f;
    private bool lastWasGap = false;
    private bool lastWasRamp = false;

    void Start()
    {
        Transform refEnd = startObject.transform.Find("End");
        currentPosition = refEnd.position;
        currentYAngle = refEnd.eulerAngles.y;
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, currentPosition) < 200f)
            SpawnNewTiles();
    }

    private GameObject GetStraightObstacleToSpawn()
    {
        bool randomObstacle = Random.value < 0.7f;
        if (randomObstacle && straightForwardWithHoles.Count > 0)
        {
            int index = Random.Range(0, straightForwardWithHoles.Count);
            return straightForwardWithHoles[index];
        }
        else if (slideUnderObstacles.Count > 0)
        {
            Debug.Log("Spawning a slide-under obstacle");
            int index = Random.Range(0, slideUnderObstacles.Count);
            return slideUnderObstacles[index];
        }
        return straightForward;
    }

    private GameObject GetRampToSpawn()
    {
        bool spawnUp = Random.value < 0.5f;
        if (spawnUp && rampUpPrefabs.Count > 0)
        {
            int index = Random.Range(0, rampUpPrefabs.Count);
            return rampUpPrefabs[index];
        }
        else if (rampDownPrefabs.Count > 0)
        {
            int index = Random.Range(0, rampDownPrefabs.Count);
            return rampDownPrefabs[index];
        }
        return null;
    }

    private void SpawnNewTiles()
    {
        GameObject prefabToSpawn;
        bool randomChance = Random.value < 0.2f;
        bool spawnCorner = false;
        bool turnRightNext = Random.value > 0.5f;

        // Decide if we should spawn a corner
        if (straightStreakCount >= 5 && randomChance && !lastWasCorner && !lastWasRamp)
            spawnCorner = true;

        if (spawnCorner)
        {
            prefabToSpawn = turnRightNext ? turnRight : turnLeft;
            lastWasCorner = true;
            lastWasRamp = false;
            straightStreakCount = 0;
        }
        else
        {
            prefabToSpawn = straightForward;
            lastWasCorner = false;
            straightStreakCount++;

            // 20% chance to spawn an obstacle (if available and not after a gap)
            if (Random.value < 0.2f && straightForwardWithHoles.Count > 0 && !lastWasGap)
                prefabToSpawn = GetStraightObstacleToSpawn();

            // 10% chance to spawn a ramp (if available, not after a corner or ramp)
            if (!lastWasRamp && !lastWasCorner && (rampUpPrefabs.Count > 0 || rampDownPrefabs.Count > 0))
            {
                if (Random.value < 0.1f)
                {
                    GameObject rampPrefab = GetRampToSpawn();
                    if (rampPrefab != null)
                    {
                        prefabToSpawn = rampPrefab;
                        lastWasRamp = true;
                        straightStreakCount = 0;
                    }
                }
                else
                {
                    lastWasRamp = false;
                }
            }
            else
            {
                lastWasRamp = false;
            }
        }

        // 9% chance to spawn a gap (only for straight tiles, not after a gap)
        if (prefabToSpawn == straightForward && Random.value < 0.09f && !lastWasGap)
        {
            Vector3 direction = Quaternion.Euler(0, currentYAngle, 0) * Vector3.forward;
            currentPosition += direction * gapDistance;
            lastWasGap = true;
            return;
        }

        Quaternion rotation = Quaternion.Euler(0, currentYAngle, 0);
        Vector3 spawnPos = currentPosition;
        Vector3Int checkPos = Vector3Int.RoundToInt(spawnPos);
        GameObject newTile = Instantiate(prefabToSpawn, spawnPos, rotation);
        Transform foundEnd = newTile.transform.Find("End");

        if (foundEnd == null)
        {
            Destroy(newTile);
            return;
        }

        TileEndTrigger trigger = foundEnd.GetComponent<TileEndTrigger>();
        if (trigger != null)
            trigger.parentTile = newTile;

        Vector3 newEndPosition = foundEnd.position;
        currentPosition = newEndPosition;

        if (prefabToSpawn == turnRight)
            currentYAngle += 90f;
        else if (prefabToSpawn == turnLeft)
            currentYAngle -= 90f;

        if (lastWasGap)
            lastWasGap = false;
    }
}
