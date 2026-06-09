using UnityEngine;

/// Handles spawning enemies at random spawn points.
/// 
/// The WaveManager tells this script when to spawn enemies.
public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]

    [Tooltip("The default enemy prefab that will be spawned.")]
    [SerializeField] private GameObject defaultEnemyPrefab;

    [Header("Spawn Points")]

    [Tooltip("The locations where enemies can spawn.")]
    [SerializeField] private Transform[] spawnPoints;

    /// Spawns one enemy using the default enemy prefab.
    public GameObject SpawnDefaultEnemy()
    {
        return SpawnEnemy(defaultEnemyPrefab);
    }

    /// Spawns one enemy using a specific enemy prefab.
    /// This will be useful later when waves include different enemy types.
    public GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        // Safety check: make sure an enemy prefab exists.
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner is missing an enemy prefab.");
            return null;
        }

        // Safety check: make sure at least one spawn point exists.
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("EnemySpawner has no spawn points assigned.");
            return null;
        }

        // Choose a random spawn point.
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawnPoint = spawnPoints[randomIndex];

        // Spawn the enemy at the selected spawn point.
        GameObject newEnemy = Instantiate(
            enemyPrefab,
            selectedSpawnPoint.position,
            selectedSpawnPoint.rotation
        );

        return newEnemy;
    }
}