using System;
using System.Collections;
using UnityEngine;

/// Handles enemy spawning in the arena.
/// 
/// This spawner supports normal spawn points and special pattern enemy spawn points.
/// Pattern enemies spawn away from the arena edges so their movement pattern stays inside the arena.
public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]

    [Tooltip("Default enemy prefab used by simple spawn calls.")]
    [SerializeField] private GameObject defaultEnemyPrefab;

    [Header("Normal Spawn Points")]

    [Tooltip("Possible locations where normal enemies can spawn.")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Pattern Enemy Spawn Points")]

    [Tooltip("Safer spawn points used by pattern movement enemies.")]
    [SerializeField] private Transform[] patternSpawnPoints;

    [Header("Spawn Indicator Settings")]

    [Tooltip("Prefab used as the warning indicator before enemies spawn.")]
    [SerializeField] private GameObject spawnIndicatorPrefab;

    [Tooltip("How long the indicator stays visible before the enemy appears.")]
    [SerializeField] private float indicatorDuration = 1f;

    /// Spawns the default enemy immediately.
    /// Useful for quick testing.
    public GameObject SpawnDefaultEnemy()
    {
        return SpawnEnemy(defaultEnemyPrefab);
    }

    /// Spawns an enemy immediately at the correct type of spawn point.
    /// Normal enemies use normal spawn points.
    /// Pattern enemies use pattern spawn points.
    public GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner is missing enemy prefab.");
            return null;
        }

        Transform spawnPoint = GetSpawnPointForEnemy(enemyPrefab);

        if (spawnPoint == null)
        {
            Debug.LogWarning("EnemySpawner has no valid spawn points assigned.");
            return null;
        }

        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        return enemy;
    }

    /// Starts a spawn with a warning indicator.
    /// 
    /// The enemy appears only after the indicator duration finishes.
    /// The WaveManager receives the spawned enemy through the callback.
    public Coroutine SpawnEnemyWithIndicator(GameObject enemyPrefab, Action<GameObject> onEnemySpawned)
    {
        return StartCoroutine(SpawnEnemyWithIndicatorRoutine(enemyPrefab, onEnemySpawned));
    }

    /// Shows an indicator, waits, then spawns the enemy.
    private IEnumerator SpawnEnemyWithIndicatorRoutine(GameObject enemyPrefab, Action<GameObject> onEnemySpawned)
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner is missing enemy prefab.");
            yield break;
        }

        Transform spawnPoint = GetSpawnPointForEnemy(enemyPrefab);

        if (spawnPoint == null)
        {
            Debug.LogWarning("EnemySpawner has no valid spawn points assigned.");
            yield break;
        }

        // Create the visual warning indicator.
        if (spawnIndicatorPrefab != null)
        {
            GameObject indicatorObject = Instantiate(
                spawnIndicatorPrefab,
                spawnPoint.position,
                Quaternion.identity
            );

            // Supports the new ring indicator.
            SpawnIndicatorRing ringIndicator = indicatorObject.GetComponent<SpawnIndicatorRing>();

            // Supports the older solid circle indicator if you still have it.
            SpawnIndicator solidIndicator = indicatorObject.GetComponent<SpawnIndicator>();

            if (ringIndicator != null)
            {
                ringIndicator.StartIndicator(indicatorDuration);
            }
            else if (solidIndicator != null)
            {
                solidIndicator.StartIndicator(indicatorDuration);
            }
            else
            {
                Destroy(indicatorObject, indicatorDuration);
            }
        }

        // Wait while the indicator is visible.
        yield return new WaitForSeconds(indicatorDuration);

        // Spawn the enemy after the warning indicator finishes.
        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        if (onEnemySpawned != null)
        {
            onEnemySpawned(enemy);
        }
    }

    /// Chooses the correct spawn point list based on enemy type.
    /// PatternMover enemies use pattern spawn points.
    /// Other enemies use normal spawn points.
    private Transform GetSpawnPointForEnemy(GameObject enemyPrefab)
    {
        if (IsPatternMoverEnemy(enemyPrefab))
        {
            Transform patternSpawnPoint = GetRandomSpawnPoint(patternSpawnPoints);

            if (patternSpawnPoint != null)
            {
                return patternSpawnPoint;
            }

            Debug.LogWarning("Pattern enemy has no Pattern Spawn Points assigned. Falling back to normal spawn points.");
        }

        return GetRandomSpawnPoint(spawnPoints);
    }

    /// Checks if the enemy prefab has the PatternMoverEnemy script.
    private bool IsPatternMoverEnemy(GameObject enemyPrefab)
    {
        return enemyPrefab.GetComponent<PatternMoverEnemy>() != null;
    }

    /// Returns a random spawn point from a given spawn point list.
    private Transform GetRandomSpawnPoint(Transform[] points)
    {
        if (points == null || points.Length == 0)
        {
            return null;
        }

        int randomIndex = UnityEngine.Random.Range(0, points.Length);

        return points[randomIndex];
    }
}