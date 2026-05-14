using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnEnemies : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Spawn Points")]
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [Header("Timing")]
    [SerializeField] private float spawnInterval = 30f;

    private void Start()
    {
        StartCoroutine(SpawnLoop());
    }

    private IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnWave();
            yield return new WaitForSeconds(spawnInterval);
        }
    }

    private void SpawnWave()
    {
        if (enemyPrefab == null || spawnPoints.Count == 0)
        {
            Debug.LogWarning("Spawner missing prefab or spawn points!");
            return;
        }

        foreach (Transform point in spawnPoints)
        {
            int amount = Random.Range(8, 11); // 8–10 inclusive

            for (int i = 0; i < amount; i++)
            {
                Instantiate(enemyPrefab, point.position, point.rotation);
            }
        }
    }
}
