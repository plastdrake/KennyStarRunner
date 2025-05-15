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
            bool turnRightNext = Random.value > 0.5f;

            Quaternion rotation = Quaternion.identity;
            if (randomChance && !lastWasCorner)
                spawnCorner = true;

            if (spawnCorner)
            {
                prefabToSpawn = turnRight;

                Vector3 oldDirection = direction;
                direction = GetTurnedDirection(direction, turnRightNext);
                Vector3 offset = (oldDirection + direction).normalized * (tileLength / 2f);
                spawnPos = previousPosition + offset;
                lastWasCorner = true;

                float angle = 0f;

                if (direction == Vector3.forward)
                    angle = turnRightNext ? -90f : 90f;
                
                else if (direction == Vector3.back)
                    angle = turnRightNext ? 90f : -90f;
                

                rotation = Quaternion.Euler(0, angle, 0);
            }
            else
            {
                prefabToSpawn = GetStraightPrefab(direction);

                spawnPos = previousPosition + direction * tileLength;
                lastWasCorner = false;
            }


            Instantiate(prefabToSpawn, spawnPos, rotation);
            previousPosition = spawnPos;
            startTime = Time.time;
            
        }
    }

    GameObject GetStraightPrefab(Vector3 dir)
    {
        if (dir == Vector3.forward || dir == Vector3.back)
            return straightForward;

        return straightRight;
    }

    Vector3 GetTurnedDirection(Vector3 current, bool right)
    {
        if (current == Vector3.forward)
            return right ? Vector3.right : Vector3.left;
        

        if (current == Vector3.right)
            return right ? Vector3.back : Vector3.forward;
        

        if (current == Vector3.back)
            return right ? Vector3.left : Vector3.right;
        

        if (current == Vector3.left) 
            return right ? Vector3.forward : Vector3.back;

        return current;
    }
}
