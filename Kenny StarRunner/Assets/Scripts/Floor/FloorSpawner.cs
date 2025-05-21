using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    public GameObject straightForward;
    public GameObject turnLeft;
    public GameObject turnRight;
    public GameObject referenceObject;
    public GameObject player;
    public float timeOffset = 1f;

    private float maxSpawnDistance = 50f;
    private Vector3 currentPosition;
    private float currentYAngle;
    private float startTime;
    private bool lastWasCorner = false;
    private int straightStreakCount = 0;

    void Start()
    {
        if (straightForward is null) Debug.Log("Straight är null");
        Transform refEnd = referenceObject.transform.Find("End");
        if (refEnd == null)
        {
            Debug.LogError("Reference object missing End snap point.");
            return;
        }

        currentPosition = refEnd.position;
        currentYAngle = refEnd.eulerAngles.y;
        startTime = Time.time;
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, currentPosition) < maxSpawnDistance)
            SpawnNewTiles();
    }

    private void SpawnNewTiles()
    {
        if (Time.time - startTime > timeOffset)
        {
            GameObject prefabToSpawn;
            bool randomChance = Random.value < 0.2f;
            bool spawnCorner = false;
            bool turnRightNext = Random.value > 0.5f;

            if (straightStreakCount >= 5 && randomChance && !lastWasCorner)
                spawnCorner = true;

            if (spawnCorner)
            {
                prefabToSpawn = turnRightNext ? turnRight : turnLeft;
                lastWasCorner = true;
                straightStreakCount = 0;
            }
            else
            {
                prefabToSpawn = straightForward;
                lastWasCorner = false;
                straightStreakCount++;
            }
            Quaternion rotation = Quaternion.Euler(0, currentYAngle, 0);
            Vector3 spawnPos = currentPosition;
            spawnPos.y = 0f;
            GameObject newTile = Instantiate(prefabToSpawn, currentPosition, rotation);
            Transform foundEnd = newTile.transform.Find("End");

            if (foundEnd == null)
            {
                Destroy(newTile);
                return;
            }

            TileEndTrigger trigger = foundEnd.GetComponent<TileEndTrigger>();
            if (trigger != null)
                trigger.parentTile = newTile;


            // Koppla End-trigger till sin tile
            Vector3 newEndPosition = foundEnd.position;
            newEndPosition.y = 0f;
            currentPosition = newEndPosition;

            if (prefabToSpawn == turnRight)
                currentYAngle += 90f;
            else if (prefabToSpawn == turnLeft)
                currentYAngle -= 90f;

            startTime = Time.time;
        }
    }
}
