using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    public GameObject straightForward;
    public GameObject straightRight;
    public GameObject turnLeft;
    public GameObject turnRight;
    public GameObject referenceObject;

    public float timeOffset = 0.4f;

    private Vector3 previousPosition;
    private Vector3 direction = Vector3.forward;
    private float tileLength = 10f;
    private float startTime;
    private bool lastWasCorner = false;

    void Start()
    {
        previousPosition = referenceObject.transform.position;
        startTime = Time.time;
    }

    void Update()
    {
        if (Time.time - startTime > timeOffset)
        {
            GameObject prefabToSpawn;
            Vector3 spawnPos;

            bool randomChance = Random.value < 0.2f;
            bool spawnCorner = false;
            if (randomChance && !lastWasCorner)
                spawnCorner = true;

            if (spawnCorner)
            {
                bool turnRightNext = Random.value > 0.5f;
                if (turnRightNext)
                    prefabToSpawn = turnRight;
                else
                    prefabToSpawn = turnLeft;

                Vector3 oldDirection = direction;
                direction = GetTurnedDirection(direction, turnRightNext);
                Vector3 offset = (oldDirection + direction).normalized * (tileLength / 2f);
                spawnPos = previousPosition + offset;
                lastWasCorner = true;
            }
            else
            {
                prefabToSpawn = GetStraightPrefab(direction);
                spawnPos = previousPosition + direction * tileLength;
                lastWasCorner = false;
            }

            Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
            previousPosition = spawnPos;
            startTime = Time.time;
        }
    }

    GameObject GetStraightPrefab(Vector3 dir)
    {
        if (dir == Vector3.forward || dir == Vector3.back)
            return straightForward;
        else
            return straightRight;
    }

    Vector3 GetTurnedDirection(Vector3 current, bool right)
    {
        if (current == Vector3.forward)
        {
            if (right)
                return Vector3.right;
            else
                return Vector3.left;
        }
        else if (current == Vector3.right)
        {
            if (right)
                return Vector3.back;
            else
                return Vector3.forward;
        }
        else if (current == Vector3.back)
        {
            if (right)
                return Vector3.left;
            else
                return Vector3.right;
        }
        else if (current == Vector3.left)
        {
            if (right)
                return Vector3.forward;
            else
                return Vector3.back;
        }
        else
        {
            return current;
        }
    }
}
