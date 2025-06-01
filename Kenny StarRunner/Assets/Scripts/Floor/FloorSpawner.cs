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

    private Vector3 currentPosition;
    private float currentYAngle;
    private bool lastWasCorner = false;
    private int straightStreakCount = 0;
    private float gapDistance = 8f;
    private bool lastWasGap = false;
    private bool lastWasRamp = false;
    private bool lastWasSlideUnder = false;

    private int currentHeightLevel = 0;
    private Dictionary<Vector2Int, List<int>> occupiedPositions = new Dictionary<Vector2Int, List<int>>();
    private float platformLifetime = 60f;
    private float lastTileLength = 8f; // Default, will be updated dynamically

    // Increased minimum separation for crossings
    private const int crossingMinSeparation = 6;

    // Buffer: how many tiles ahead of the player should always exist
    [SerializeField] private int tilesAheadBuffer = 6;
    // Track spawned tiles for buffer management
    private Queue<GameObject> spawnedTiles = new Queue<GameObject>();

    void Start()
    {
        Transform refEnd = startObject.transform.Find("End");
        currentPosition = refEnd.position;
        currentYAngle = refEnd.eulerAngles.y;
        lastTileLength = (refEnd.position - startObject.transform.position).magnitude;
        currentHeightLevel = Mathf.RoundToInt(currentPosition.y / lastTileLength);
        RegisterTile(currentPosition, currentHeightLevel, lastTileLength);

        // Spawn initial buffer
        for (int i = 0; i < tilesAheadBuffer; i++)
            SpawnNewTiles();
    }

    // Called by TileEndTrigger when player passes through an End trigger
    public void OnTileEndReached(TileEndTrigger trigger)
    {
        // Remove the tile that was just passed (if it's in the queue)
        if (trigger.parentTile != null && spawnedTiles.Count > 0 && spawnedTiles.Peek() == trigger.parentTile)
            spawnedTiles.Dequeue();

        // Spawn enough tiles to refill the buffer
        int toSpawn = tilesAheadBuffer - spawnedTiles.Count;
        for (int i = 0; i < toSpawn; i++)
            SpawnNewTiles();
    }

    private GameObject GetStraightObstacleToSpawn(bool preventSlideUnder)
    {
        bool randomObstacle = Random.value < 0.7f;
        if (randomObstacle && straightForwardWithHoles.Count > 0)
        {
            int index = Random.Range(0, straightForwardWithHoles.Count);
            return straightForwardWithHoles[index];
        }
        else if (!preventSlideUnder && slideUnderObstacles.Count > 0)
        {
            Debug.Log("Spawning a slide-under obstacle");
            int index = Random.Range(0, slideUnderObstacles.Count);
            return slideUnderObstacles[index];
        }
        return straightForward;
    }

    private GameObject GetRampToSpawn(bool preferUp = false)
    {
        bool spawnUp = preferUp || Random.value < 0.5f;
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

    private void RegisterTile(Vector3 pos, int heightLevel, float tileLength)
    {
        Vector2Int pos2D = new Vector2Int(Mathf.RoundToInt(pos.x / tileLength), Mathf.RoundToInt(pos.z / tileLength));
        if (!occupiedPositions.ContainsKey(pos2D))
            occupiedPositions[pos2D] = new List<int>();
        occupiedPositions[pos2D].Add(heightLevel);
    }

    private bool IsOverlapping(Vector3 pos, int heightLevel, float tileLength, int minSeparation = 2)
    {
        Vector2Int pos2D = new Vector2Int(Mathf.RoundToInt(pos.x / tileLength), Mathf.RoundToInt(pos.z / tileLength));
        if (!occupiedPositions.ContainsKey(pos2D))
            return false;
        foreach (int h in occupiedPositions[pos2D])
        {
            if (Mathf.Abs(h - heightLevel) < minSeparation)
                return true;
        }
        return false;
    }

    private float GetTileLength(GameObject prefab)
    {
        GameObject temp = Instantiate(prefab, Vector3.zero, Quaternion.identity);
        Transform end = temp.transform.Find("End");
        float length = end != null ? (end.position - temp.transform.position).magnitude : 8f;
        Destroy(temp);
        return length;
    }

    private void SpawnNewTiles()
    {
        GameObject prefabToSpawn;
        bool randomChance = Random.value < 0.5f;
        bool spawnCorner = false;
        bool turnRightNext = Random.value > 0.5f;

        if (straightStreakCount >= 3 && randomChance && !lastWasCorner && !lastWasRamp)
            spawnCorner = true;

        // Predict next position if a turn is spawned
        GameObject turnPrefab = turnRightNext ? turnRight : turnLeft;
        float turnTileLength = GetTileLength(turnPrefab);
        Vector3 direction = Quaternion.Euler(0, currentYAngle + (turnRightNext ? 90f : -90f), 0) * Vector3.forward;
        Vector3 predictedTurnPos = currentPosition + direction * turnTileLength;
        int predictedHeight = currentHeightLevel;

        if (spawnCorner)
        {
            int rampsNeeded = 0;
            if (IsOverlapping(predictedTurnPos, predictedHeight, turnTileLength, crossingMinSeparation))
                rampsNeeded = Random.Range(2, 4); // 2 or 3 ramps

            for (int i = 0; i < rampsNeeded; i++)
            {
                bool goUp = (currentHeightLevel < 10 && (currentHeightLevel == 0 || Random.value < 0.5f));
                GameObject rampPrefab = goUp && rampUpPrefabs.Count > 0
                    ? rampUpPrefabs[Random.Range(0, rampUpPrefabs.Count)]
                    : rampDownPrefabs.Count > 0
                        ? rampDownPrefabs[Random.Range(0, rampDownPrefabs.Count)]
                        : null;
                if (rampPrefab == null) break;

                Quaternion rotation = Quaternion.Euler(0, currentYAngle, 0);
                Vector3 spawnPos = currentPosition;
                GameObject newTile = Instantiate(rampPrefab, spawnPos, rotation);
                Destroy(newTile, platformLifetime);

                Transform foundEnd = newTile.transform.Find("End");
                float rampTileLength = foundEnd != null ? (foundEnd.position - newTile.transform.position).magnitude : 8f;
                if (foundEnd == null)
                {
                    Destroy(newTile);
                    break;
                }
                TileEndTrigger trigger = foundEnd.GetComponent<TileEndTrigger>();
                if (trigger != null)
                {
                    trigger.parentTile = newTile;
                    trigger.spawner = this;
                }

                currentPosition = foundEnd.position;
                currentHeightLevel += goUp ? 1 : -1;
                RegisterTile(currentPosition, currentHeightLevel, rampTileLength);

                spawnedTiles.Enqueue(newTile);

                // Update predictedTurnPos and predictedHeight after each ramp
                predictedHeight = currentHeightLevel;
                turnTileLength = GetTileLength(turnPrefab);
                direction = Quaternion.Euler(0, currentYAngle + (turnRightNext ? 90f : -90f), 0) * Vector3.forward;
                predictedTurnPos = currentPosition + direction * turnTileLength;
            }

            if (IsOverlapping(predictedTurnPos, predictedHeight, turnTileLength, crossingMinSeparation))
            {
                prefabToSpawn = straightForward;
                lastWasCorner = false;
                straightStreakCount++;
            }
            else
            {
                prefabToSpawn = turnPrefab;
                lastWasCorner = true;
                lastWasRamp = false;
                straightStreakCount = 0;
            }
        }
        else
        {
            prefabToSpawn = straightForward;
            lastWasCorner = false;
            straightStreakCount++;

            // 20% chance to spawn an obstacle (if available and not after a gap)
            if (Random.value < 0.2f && (straightForwardWithHoles.Count > 0 || slideUnderObstacles.Count > 0) && !lastWasGap)
                prefabToSpawn = GetStraightObstacleToSpawn(lastWasSlideUnder);

            // 25% chance to spawn a ramp (increased from 10%)
            if (!lastWasRamp && !lastWasCorner && (rampUpPrefabs.Count > 0 || rampDownPrefabs.Count > 0))
            {
                if (Random.value < 0.25f)
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
            float straightTileLength = GetTileLength(straightForward);
            Vector3 gapDirection = Quaternion.Euler(0, currentYAngle, 0) * Vector3.forward;
            currentPosition += gapDirection * gapDistance;
            lastWasGap = true;
            lastWasSlideUnder = false;
            return;
        }

        Quaternion finalRotation = Quaternion.Euler(0, currentYAngle, 0);
        Vector3 spawnPosFinal = currentPosition;
        GameObject newTileFinal = Instantiate(prefabToSpawn, spawnPosFinal, finalRotation);
        Destroy(newTileFinal, platformLifetime);

        Transform foundEndFinal = newTileFinal.transform.Find("End");
        float thisTileLength = foundEndFinal != null ? (foundEndFinal.position - newTileFinal.transform.position).magnitude : 8f;
        if (foundEndFinal == null)
        {
            Destroy(newTileFinal);
            lastWasSlideUnder = false;
            return;
        }

        TileEndTrigger triggerFinal = foundEndFinal.GetComponent<TileEndTrigger>();
        if (triggerFinal != null)
        {
            triggerFinal.parentTile = newTileFinal;
            triggerFinal.spawner = this; // <-- This sets the reference at runtime!
        }

        Vector3 newEndPosition = foundEndFinal.position;
        currentPosition = newEndPosition;

        if (rampUpPrefabs.Contains(prefabToSpawn))
            currentHeightLevel++;
        else if (rampDownPrefabs.Contains(prefabToSpawn))
            currentHeightLevel--;

        RegisterTile(currentPosition, currentHeightLevel, thisTileLength);

        spawnedTiles.Enqueue(newTileFinal);

        if (prefabToSpawn == turnRight)
            currentYAngle += 90f;
        else if (prefabToSpawn == turnLeft)
            currentYAngle -= 90f;

        // Set slide-under flag
        lastWasSlideUnder = slideUnderObstacles.Contains(prefabToSpawn);

        if (lastWasGap)
            lastWasGap = false;

        // Optionally, clean up old tiles from the queue if needed
        while (spawnedTiles.Count > tilesAheadBuffer + 5) // keep a few extra for safety
        {
            var oldTile = spawnedTiles.Dequeue();
            if (oldTile != null)
                Destroy(oldTile);
        }
    }
}
