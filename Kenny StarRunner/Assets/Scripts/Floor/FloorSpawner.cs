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
            GameObject prefabToSpawn = GetStraightPrefab(direction);

            if (Random.value < 0.2f && !lastWasCorner)
            {
                bool turnRightNext = Random.value > 0.5f;
                prefabToSpawn = turnRightNext ? turnRight : turnLeft;

                direction = GetTurnedDirection(direction, turnRightNext);
                lastWasCorner = true;
            }
            else
            {
                lastWasCorner = false;
            }

            Vector3 spawnPos = previousPosition + direction * tileLength;
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
        if (current == Vector3.forward) return right ? Vector3.right : Vector3.left;
        if (current == Vector3.right) return right ? Vector3.back : Vector3.forward;
        if (current == Vector3.back) return right ? Vector3.left : Vector3.right;
        if (current == Vector3.left) return right ? Vector3.forward : Vector3.back;
        return current;
    }
}
