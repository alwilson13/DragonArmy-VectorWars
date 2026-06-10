using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// Controls the wave progression for Vector wars.
/// 
/// The WaveManager starts waves, tells the EnemySpawner to spawn enemies,
/// tracks active enemies, and starts the next wave after all enemies are defeated.
/// 
/// Current wave trigger:
/// Kill All - the next wave starts when all enemies in the current wave are dead.
public class WaveManager : MonoBehaviour
{
    [System.Serializable]
    public class WaveData
    {
        [Tooltip("Name of the wave for debugging and later UI.")]
        public string waveName;

        [Tooltip("Enemy prefab used for this wave.")]
        public GameObject enemyPrefab;

        [Tooltip("How many enemies should spawn in this wave.")]
        public int enemyCount = 5;

        [Tooltip("How much time passes between each enemy spawn.")]
        public float spawnDelay = 0.5f;
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
        // If EnemySpawner was not assigned in the Inspector, try to find it on this object.
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

    /// Starts the current wave and spawns all enemies for that wave.
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

        // Clear old missing references before starting the new wave.
        activeEnemies.Clear();

        // Spawn enemies one at a time.
        for (int i = 0; i < currentWave.enemyCount; i++)
        {
            GameObject spawnedEnemy = enemySpawner.SpawnEnemy(currentWave.enemyPrefab);

            // Add the spawned enemy to the active enemy list.
            if (spawnedEnemy != null)
            {
                activeEnemies.Add(spawnedEnemy);
            }

            yield return new WaitForSeconds(currentWave.spawnDelay);
        }
    }

    private void Update()
    {
        // Only check wave completion if a wave is currently active.
        if (waveInProgress)
        {
            CheckWaveCompletion();
        }
    }

    /// Checks if all enemies in the current wave are dead.
    /// If they are, start the next wave.
    private void CheckWaveCompletion()
    {
        // Remove enemies from the list if they have been destroyed.
        activeEnemies.RemoveAll(enemy => enemy == null);

        // If no active enemies remain, the wave is complete.
        if (activeEnemies.Count == 0)
        {
            waveInProgress = false;

            Debug.Log("Wave " + (currentWaveIndex + 1) + " complete.");

            currentWaveIndex++;

            StartCoroutine(StartNextWaveAfterDelay());
        }
    }

    /// Waits a short time before starting the next wave.
    private IEnumerator StartNextWaveAfterDelay()
    {
        yield return new WaitForSeconds(delayBetweenWaves);

        StartCoroutine(StartWaveRoutine());
    }

    /// Handles the victory condition after the final wave is cleared.
    /// Later, this will open the Victory UI screen.
    private void Victory()
    {
        Debug.Log("Victory! All waves cleared.");

        // Show the Victory UI screen.
        UIManager uiManager = FindFirstObjectByType<UIManager>();

        if (uiManager != null)
        {
            uiManager.ShowVictory();
        }
    }

    /// Returns the current wave number for UI later.
    public int GetCurrentWaveNumber()
    {
        return currentWaveIndex + 1;
    }

    /// Returns the total number of waves.
    public int GetTotalWaves()
    {
        return waves.Length;
    }
}