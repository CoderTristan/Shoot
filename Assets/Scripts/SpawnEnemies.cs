using UnityEngine;
using System.Collections.Generic;

public class SpawnEnemies : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int enemyCount = 5;

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    private void Start()
    {
        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner: No enemy prefab assigned!");
            return;
        }

        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("EnemySpawner: No spawn points assigned!");
            return;
        }

        for (int i = 0; i < enemyCount; i++)
        {
            Transform point = spawnPoints[i % spawnPoints.Count];
            Instantiate(enemyPrefab, point.position, point.rotation);
        }
    }
}
