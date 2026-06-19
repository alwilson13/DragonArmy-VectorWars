using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Controls wave progression for Vector wars.
/// 
/// This version supports mixed waves.
/// Each wave can contain multiple enemy groups.
/// Example:
/// Wave 3 can spawn 5 Basic Chasers and 2 Chargers.
public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class EnemyGroup
    {
        [Tooltip("Enemy prefab used for this group.")]
        public GameObject enemyPrefab;

        [Tooltip("How many enemies of this type should spawn.")]
        public int enemyCount = 5;

        [Tooltip("How much time passes between each enemy spawn in this group.")]
        public float spawnDelay = 0.5f;
    }

    [System.Serializable]
    public class WaveData
    {
        [Tooltip("Name of the wave for debugging and UI.")]
        public string waveName;

        [Tooltip("Enemy groups included in this wave.")]
        public EnemyGroup[] enemyGroups;
    }

    [Header("Wave Settings")]

    [Tooltip("List of waves the player must clear.")]
    [SerializeField] private WaveData[] waves;

    [Tooltip("How long to wait before the first wave starts.")]
    [SerializeField] private float firstWaveDelay = 1f;

    [Tooltip("How long to wait between waves.")]
    [SerializeField] private float delayBetweenWaves = 2f;

    [Header("References")]

    [Tooltip("Reference to the EnemySpawner in the scene.")]
    [SerializeField] private EnemySpawner enemySpawner;

    // The index of the current wave.
    private int currentWaveIndex = 0;

    // List of enemies currently alive in the active wave.
    private readonly List<GameObject> activeEnemies = new List<GameObject>();

    // Tracks whether a wave is currently running.
    private bool waveInProgress;

    private void Awake()
    {
        // If EnemySpawner was not assigned, try to find it on this object.
        if (enemySpawner == null)
        {
            enemySpawner = GetComponent<EnemySpawner>();
        }
    }

    private void Start()
    {
        // Start the first wave after a short delay.
        StartCoroutine(StartFirstWaveRoutine());
    }

    /// Waits briefly, then starts the first wave.
    private IEnumerator StartFirstWaveRoutine()
    {
        yield return new WaitForSeconds(firstWaveDelay);

        StartCoroutine(StartWaveRoutine());
    }

    /// Starts the current wave and spawns all enemy groups inside it.
    private IEnumerator StartWaveRoutine()
    {
        // If there are no more waves, the player wins.
        if (currentWaveIndex >= waves.Length)
        {
            Victory();
            yield break;
        }

        waveInProgress = true;

        WaveData currentWave = waves[currentWaveIndex];

        Debug.Log("Starting " + currentWave.waveName);

        activeEnemies.Clear();

        // Loop through each enemy group in the wave.
        for (int groupIndex = 0; groupIndex < currentWave.enemyGroups.Length; groupIndex++)
        {
            EnemyGroup group = currentWave.enemyGroups[groupIndex];

            // Spawn all enemies in this group.
            for (int enemyIndex = 0; enemyIndex < group.enemyCount; enemyIndex++)
            {
                GameObject spawnedEnemy = enemySpawner.SpawnEnemy(group.enemyPrefab);

                if (spawnedEnemy != null)
                {
                    activeEnemies.Add(spawnedEnemy);
                }

                yield return new WaitForSeconds(group.spawnDelay);
            }
        }
    }

    private void Update()
    {
        // Only check wave completion while a wave is active.
        if (waveInProgress)
        {
            CheckWaveCompletion();
        }
    }

    /// Checks if all enemies in the current wave are dead.
    /// If they are, either starts the next wave or prepares Victory after rewards.
    private void CheckWaveCompletion()
    {
        // Remove destroyed enemies from the active enemy list.
        activeEnemies.RemoveAll(enemy => enemy == null);

        // If enemies are still alive, the wave is not complete.
        if (activeEnemies.Count > 0)
        {
            return;
        }

        waveInProgress = false;

        Debug.Log("Wave " + (currentWaveIndex + 1) + " complete.");

        currentWaveIndex++;

        // If this was the final wave, wait for XP orbs and level-up before Victory.
        if (currentWaveIndex >= waves.Length)
        {
            StartCoroutine(WaitForRewardsThenVictory());
        }
        else
        {
            StartCoroutine(StartNextWaveAfterDelay());
        }
    }

    /// Waits before starting the next wave.
    private IEnumerator StartNextWaveAfterDelay()
    {
        yield return new WaitForSeconds(delayBetweenWaves);

        StartCoroutine(StartWaveRoutine());
    }

    /// Handles victory after the final wave is cleared.
    private void Victory()
    {
        PlayerHealth playerHealth = FindFirstObjectByType<PlayerHealth>();

        // If the player is dead, do not trigger victory.
        if (playerHealth != null && playerHealth.IsDead())
        {
            return;
        }

        Debug.Log("Victory! All waves cleared.");

        UIManager uiManager = FindFirstObjectByType<UIManager>();

        if (uiManager != null)
        {
            uiManager.ShowVictory();
        }
    }

    /// Returns the current wave number for UI.
    /// Clamps the value so it does not go above the total wave count.
    public int GetCurrentWaveNumber()
    {
        return Mathf.Clamp(currentWaveIndex + 1, 1, waves.Length);
    }

    /// Returns the total number of waves.
    public int GetTotalWaves()
    {
        return waves.Length;
    }

    /// Waits until remaining reward orbs are collected or gone,
    /// then shows victory
    private IEnumerator WaitForRewardsThenVictory()
    {
        Debug.Log("Final wave cleared. Waiting for remaining reward orbs.");

        while (FindObjectsByType<XPOrb>(FindObjectsSortMode.None).Length > 0 ||
               FindObjectsByType<UpgradeOrb>(FindObjectsSortMode.None).Length > 0 ||
               FindObjectsByType<WeaponPickup>(FindObjectsSortMode.None).Length > 0)
        {
            yield return null;
        }

        Victory();
    }
}