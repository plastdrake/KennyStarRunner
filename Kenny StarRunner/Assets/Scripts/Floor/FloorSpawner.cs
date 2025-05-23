using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    public GameObject straightForward;
    public List<GameObject> straightFormardWithHoles;
    public List<GameObject> slideUnderObsticles;
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

    void Start()
    {
        Transform refEnd = startObject.transform.Find("End");
        currentPosition = refEnd.position;
        currentYAngle = refEnd.eulerAngles.y;
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, currentPosition) < 50f)
            SpawnNewTiles();
    }

    private GameObject StraightObsticleToSpawn()
    {
        GameObject prefab;
        bool randomObsticle = Random.value < 0.7f;
        if(randomObsticle)
        {
            int index = Random.Range(0, straightFormardWithHoles.Count);
            prefab = straightFormardWithHoles[index];
            return prefab;
        }
        else
        {
            Debug.Log("Här ska det vara ett slide under hinder");
            int index = Random.Range(0, slideUnderObsticles.Count);
            prefab = slideUnderObsticles[index];
            return prefab;
        }
    }
    private void SpawnNewTiles()
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
            if (Random.value < 0.2f && straightFormardWithHoles.Count > 0 && !lastWasGap)
                prefabToSpawn = StraightObsticleToSpawn();
        }
        
        if (prefabToSpawn == straightForward && Random.value < 0.09f && lastWasGap == false)
        {
            Vector3 direction = Quaternion.Euler(0, currentYAngle, 0) * Vector3.forward;
            currentPosition += direction * gapDistance;
            lastWasGap = true;
            return;
        }

        Quaternion rotation = Quaternion.Euler(0, currentYAngle, 0);
        Vector3 spawnPos = currentPosition;
        spawnPos.y = 0f;
        Vector3Int checkPos = Vector3Int.RoundToInt(spawnPos);

        //if (occupiedPositions.Contains(checkPos))
        //    return;
        //occupiedPositions.Add(checkPos);
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
        newEndPosition.y = 0f;
        currentPosition = newEndPosition;

        if (prefabToSpawn == turnRight)
            currentYAngle += 90f;
        else if (prefabToSpawn == turnLeft)
            currentYAngle -= 90f;

        if (lastWasGap)
            lastWasGap = false;
    }
}
