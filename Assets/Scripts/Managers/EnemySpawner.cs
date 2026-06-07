using UnityEngine;

/// Handles enemy spawning for Vector Wars.
/// 
/// This script spawns enemies from a list of spawn points.
/// For now, it spawns Basic Chaser enemies at a repeated time interval.
public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]

    [Tooltip("The enemy prefab that will be spawned.")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Spawn Points")]

    [Tooltip("The locations where enemies can spawn.")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Spawn Settings")]

    [Tooltip("Should the spawner start spawning automatically when the game begins?")]
    [SerializeField] private bool spawnOnStart = true;

    [Tooltip("How many seconds to wait before the first enemy spawns.")]
    [SerializeField] private float firstSpawnDelay = 1f;

    [Tooltip("How many seconds between each enemy spawn.")]
    [SerializeField] private float spawnInterval = 2f;

    [Tooltip("Maximum number of enemies this spawner can create. Use -1 for unlimited.")]
    [SerializeField] private int maxEnemiesToSpawn = 10;

    // Tracks how many enemies have been spawned so far.
    private int enemiesSpawned;

    // Tracks whether the spawner is currently active.
    private bool isSpawning;

    private void Start()
    {
        // Start spawning automatically if enabled.
        if (spawnOnStart)
        {
            StartSpawning();
        }
    }

    /// Starts the repeated enemy spawning process.
    public void StartSpawning()
    {
        // Do not start again if already spawning.
        if (isSpawning)
        {
            return;
        }

        isSpawning = true;

        // Repeatedly call SpawnEnemy after an initial delay.
        InvokeRepeating(nameof(SpawnEnemy), firstSpawnDelay, spawnInterval);
    }

    /// Stops the repeated enemy spawning process.
    public void StopSpawning()
    {
        isSpawning = false;

        // Cancels the repeated SpawnEnemy calls.
        CancelInvoke(nameof(SpawnEnemy));
    }

    /// Spawns one enemy at a random spawn point.
    private void SpawnEnemy()
    {
        // If maxEnemiesToSpawn is not -1, stop after reaching the limit.
        if (maxEnemiesToSpawn != -1 && enemiesSpawned >= maxEnemiesToSpawn)
        {
            StopSpawning();
            return;
        }

        // Safety check: make sure the enemy prefab exists.
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner is missing an enemy prefab.");
            StopSpawning();
            return;
        }

        // Safety check: make sure at least one spawn point exists.
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("EnemySpawner has no spawn points assigned.");
            StopSpawning();
            return;
        }

        // Choose a random spawn point from the array.
        int randomIndex = Random.Range(0, spawnPoints.Length);
        Transform selectedSpawnPoint = spawnPoints[randomIndex];

        // Create the enemy at the selected spawn point.
        Instantiate(
            enemyPrefab,
            selectedSpawnPoint.position,
            selectedSpawnPoint.rotation
        );

        // Increase the spawned enemy count.
        enemiesSpawned++;
    }

    /// Resets the spawned enemy count.
    /// This will be useful later when waves start.
    public void ResetSpawner()
    {
        enemiesSpawned = 0;
    }
}