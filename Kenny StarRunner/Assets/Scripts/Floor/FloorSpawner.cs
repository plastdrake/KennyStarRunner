using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    public GameObject straightForward;
    public GameObject turnLeft;
    public GameObject turnRight;
    public GameObject referenceObject;

    public float timeOffset = 0.4f;

    private Vector3 currentPosition;
    private float currentYAngle;
    private float startTime;
    private bool lastWasCorner = false;

    void Start()
    {
        // Start at the reference object's End point
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
        if (Time.time - startTime > timeOffset)
        {
            GameObject prefabToSpawn;
            bool randomChance = Random.value < 0.2f;
            bool spawnCorner = false;
            bool turnRightNext = Random.value > 0.5f;

            if (randomChance && !lastWasCorner)
                spawnCorner = true;

            if (spawnCorner)
            {
                prefabToSpawn = turnRightNext ? turnRight : turnLeft;
                lastWasCorner = true;
            }
            else
            {
                prefabToSpawn = straightForward;
                lastWasCorner = false;
            }

            // Spawn the tile at the current position and accumulated rotation
            Quaternion rotation = Quaternion.Euler(0, currentYAngle, 0);
            GameObject newTile = Instantiate(prefabToSpawn, currentPosition, rotation);

            // Find the "End" snap point in the new tile
            Transform endSnap = newTile.transform.Find("End");
            if (endSnap == null)
            {
                Debug.LogError("Prefab missing End snap point.");
                Destroy(newTile);
                return;
            }

            // The next tile's Start should be at this End's world position
            currentPosition = endSnap.position;

            // Only update the accumulated rotation if a turn was placed
            if (prefabToSpawn == turnRight)
            {
                currentYAngle += 90f;
            }
            else if (prefabToSpawn == turnLeft)
            {
                currentYAngle -= 90f;
            }

            startTime = Time.time;
        }
    }
}
