using System.Collections.Generic;
using UnityEngine;

public class FloorSpawner : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public GameObject tilePrefab; // En enda prefab
    public GameObject referenceObject;

    public float timeOffset = 0.4f;
    private float startTime;
    private Vector3 direction = Vector3.forward;
    private Vector3 previousPosition;
    private float lastTileLength = 10f;

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
            bool spawnCorner = Random.value < 0.2f && !lastWasCorner; // 20% chans, ej två i rad
            lastWasCorner = spawnCorner;

            Vector3 spawnPos = previousPosition + direction * (lastTileLength / 2f);

            float length = GetRandomLength();
            Vector3 scale = new Vector3(3, 1, length);

            Quaternion rotation = Quaternion.identity;

            if (spawnCorner)
            {
                Vector3 oldDir = direction;

                // Sväng höger eller vänster
                bool turnRight = Random.value > 0.5f;
                direction = GetTurnedDirection(direction, turnRight);
                rotation = GetRotationFromDirection(direction);

                // Justera position för hörnbytet
                spawnPos += direction * (length / 2f);
            }
            else
            {
                spawnPos += direction * (length / 2f);
            }

            GameObject tile = Instantiate(tilePrefab, spawnPos, rotation);
            tile.transform.localScale = scale;

            previousPosition = spawnPos;
            lastTileLength = length;
            startTime = Time.time;
        }
    }

    float GetRandomLength()
    {
        float[] options = { 3f, 6f, 10f };
        return options[Random.Range(0, options.Length)];
    }

    Vector3 GetTurnedDirection(Vector3 current, bool right)
    {
        if (current == Vector3.forward) return right ? Vector3.right : Vector3.left;
        if (current == Vector3.right) return right ? Vector3.back : Vector3.forward;
        if (current == Vector3.back) return right ? Vector3.left : Vector3.right;
        if (current == Vector3.left) return right ? Vector3.forward : Vector3.back;
        return Vector3.forward;
    }

    Quaternion GetRotationFromDirection(Vector3 dir)
    {
        if (dir == Vector3.forward) return Quaternion.Euler(0, 0, 0);
        if (dir == Vector3.right) return Quaternion.Euler(0, 90, 0);
        if (dir == Vector3.back) return Quaternion.Euler(0, 180, 0);
        if (dir == Vector3.left) return Quaternion.Euler(0, -90, 0);
        return Quaternion.identity;
    }
}
