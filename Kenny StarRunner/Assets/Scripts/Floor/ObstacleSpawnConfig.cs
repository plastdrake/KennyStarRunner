using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ObstacleSpawnConfig", menuName = "Configs/Obstacle Spawn Config")]
public class ObstacleSpawnConfig : ScriptableObject
{
    public List<ObstaclePrefabChance> obstaclePrefabs;
}
