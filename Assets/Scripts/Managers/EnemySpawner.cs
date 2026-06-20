using System.Collections;
using UnityEngine;

/// Handles enemy spawning in the arena.
/// 
/// This spawner can show a spawn indicator before creating the enemy,
/// giving the player time to react.
public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy Prefabs")]

    [Tooltip("Default enemy prefab used by simple spawn calls.")]
    [SerializeField] private GameObject defaultEnemyPrefab;

    [Header("Spawn Points")]

    [Tooltip("Possible locations where enemies can spawn.")]
    [SerializeField] private Transform[] spawnPoints;

    [Header("Spawn Indicator Settings")]

    [Tooltip("Prefab used as the warning indicator before enemies spawn.")]
    [SerializeField] private GameObject spawnIndicatorPrefab;

    [Tooltip("How long the indicator stays visible before the enemy appears.")]
    [SerializeField] private float indicatorDuration = 1f;

    /// Spawns the default enemy immediately.
    /// Useful for simple testing.
    public GameObject SpawnDefaultEnemy()
    {
        return SpawnEnemy(defaultEnemyPrefab);
    }

    /// Spawns an enemy immediately at a random spawn point.
    public GameObject SpawnEnemy(GameObject enemyPrefab)
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner is missing enemy prefab.");
            return null;
        }

        Transform spawnPoint = GetRandomSpawnPoint();

        if (spawnPoint == null)
        {
            Debug.LogWarning("EnemySpawner has no spawn points assigned.");
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
    /// This is used by the WaveManager so enemies appear after
    /// the indicator completes.
    public Coroutine SpawnEnemyWithIndicator(GameObject enemyPrefab, System.Action<GameObject> onEnemySpawned)
    {
        return StartCoroutine(SpawnEnemyWithIndicatorRoutine(enemyPrefab, onEnemySpawned));
    }

    /// Shows indicator, waits, then spawns enemy.
    private IEnumerator SpawnEnemyWithIndicatorRoutine(GameObject enemyPrefab, System.Action<GameObject> onEnemySpawned)
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("EnemySpawner is missing enemy prefab.");
            yield break;
        }

        Transform spawnPoint = GetRandomSpawnPoint();

        if (spawnPoint == null)
        {
            Debug.LogWarning("EnemySpawner has no spawn points assigned.");
            yield break;
        }

        // Create the visual indicator at the selected spawn point.
        if (spawnIndicatorPrefab != null)
        {
            GameObject indicatorObject = Instantiate(
                spawnIndicatorPrefab,
                spawnPoint.position,
                Quaternion.identity
            );

            SpawnIndicatorRing ringIndicator = indicatorObject.GetComponent<SpawnIndicatorRing>();

            if (ringIndicator != null)
            {
                ringIndicator.StartIndicator(indicatorDuration);
            }
            else
            {
                Destroy(indicatorObject, indicatorDuration);
            }
        }

        // Wait while the indicator is visible.
        yield return new WaitForSeconds(indicatorDuration);

        // Spawn the enemy after the warning finishes.
        GameObject enemy = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        // Tell the WaveManager about the spawned enemy.
        if (onEnemySpawned != null)
        {
            onEnemySpawned(enemy);
        }
    }

    /// Returns a random spawn point from the assigned list.
    private Transform GetRandomSpawnPoint()
    {
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, spawnPoints.Length);

        return spawnPoints[randomIndex];
    }
}