using NUnit.Framework;
using UnityEngine;

public class CoinsScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject? _coinStraight;
    public GameObject? _coinDiagonal;
    public GameObject? _coinInAir;
    private GameObject? prefabToSpawn;
    private Vector3 spawnPos;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        SpawnCoin();
    }

    private void SpawnCoin()
    {
        //Måste också kika så att det är en rak prefab
        bool randomChanceToSpawnAir, randomChanceToSpawnDiagonal, randomChanceToSpawnStraight;

        randomChanceToSpawnAir = Random.value < 0.1f;
        randomChanceToSpawnDiagonal = Random.value < 0.1f;
        randomChanceToSpawnStraight = Random.value < 0.3f;
        if (randomChanceToSpawnStraight)
        {
            spawnPos = _coinStraight.transform.position;
            prefabToSpawn = _coinStraight;
            return;
        }
        else if (randomChanceToSpawnAir)
        {
            spawnPos = _coinInAir.transform.position;
            prefabToSpawn = _coinInAir;
            return;
        }
        else if (randomChanceToSpawnDiagonal)
        {
            spawnPos = _coinDiagonal.transform.position;
            prefabToSpawn = _coinDiagonal;
            return;
        }
        GameObject newCoin = Instantiate(prefabToSpawn, spawnPos, Quaternion.identity);
    }
}
