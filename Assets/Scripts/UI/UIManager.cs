using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// Handles the main gameplay UI for Vector wars.
/// 
/// This script updates health, wave, and score text.
/// It also shows the Game Over and Victory screens.
public class UIManager : MonoBehaviour
{
    [Header("HUD Text")]

    [Tooltip("Text that displays the player's current health.")]
    [SerializeField] private TMP_Text healthText;

    [Tooltip("Text that displays the current wave.")]
    [SerializeField] private TMP_Text waveText;

    [Tooltip("Text that displays the player's score.")]
    [SerializeField] private TMP_Text scoreText;

    [Tooltip("Text that displays the player's current XP.")]
    [SerializeField] private TMP_Text xpText;

    [Tooltip("Text that displays the player's current level.")]
    [SerializeField] private TMP_Text levelText;

    [Tooltip("Text that displays player combat stats.")]
    [SerializeField] private TMP_Text statsText;

    [Tooltip("Text that displays the player's current weapon.")]
    [SerializeField] private TMP_Text weaponText;

    [Header("Game Over Screen")]

    [SerializeField] private TMP_Text finalScoreText;

    [SerializeField] private TMP_Text waveReachedText;

    [Header("Victory Screen")]

    [SerializeField] private TMP_Text victoryFinalScoreText;

    [SerializeField] private TMP_Text victoryWaveReachedText;

    [Header("Panels")]

    [Tooltip("Panel shown when the player dies.")]
    [SerializeField] private GameObject gameOverPanel;

    [Tooltip("Panel shown when the player clears all waves.")]
    [SerializeField] private GameObject victoryPanel;

    [Tooltip("Panel shown when the player pauses the game.")]
    [SerializeField] private GameObject pausePanel;

    [Tooltip("Panel shown at the start of the game.")]
    [SerializeField] private GameObject launchPanel;

    [Header("References")]

    [Tooltip("Reference to the player's health script.")]
    [SerializeField] private PlayerHealth playerHealth;

    [Tooltip("Reference to the WaveManager script.")]
    [SerializeField] private WaveManager waveManager;

    [Tooltip("Reference to the ScoreManager script.")]
    [SerializeField] private ScoreManager scoreManager;

    [Tooltip("Reference to the PlayerExperience script.")]
    [SerializeField] private PlayerExperience playerExperience;

    [Tooltip("Reference to the PlayerMovement script.")]
    [SerializeField] private PlayerMovement playerMovement;

    [Tooltip("Reference to the PlayerShooting script.")]
    [SerializeField] private PlayerShooting playerShooting;


    // Prevents Game Over and Victory from showing at the same time.
    private bool gameEnded;

    // Tracks whether the game is currently paused.
    private bool isPaused;

    // Tracks whether the game is still waiting at the launch screen.
    private bool isLaunchScreenOpen = true;

    private static bool skipLaunchPanelOnRestart;

    private void Awake()
    {
        // If PlayerHealth or PlayerExperience is not assigned,
        // try to find the player by tag.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            if (playerHealth == null)
            {
                playerHealth = playerObject.GetComponent<PlayerHealth>();
            }

            if (playerExperience == null)
            {
                playerExperience = playerObject.GetComponent<PlayerExperience>();
            }

            if (playerMovement == null)
            {
                playerMovement = playerObject.GetComponent<PlayerMovement>();
            }

            if (playerShooting == null)
            {
                playerShooting = playerObject.GetComponent<PlayerShooting>();
            }
        }

        // If WaveManager is not assigned, try to find it on the same GameObject.
        if (waveManager == null)
        {
            waveManager = GetComponent<WaveManager>();
        }

        // If ScoreManager is not assigned, try to find it on the same GameObject.
        if (scoreManager == null)
        {
            scoreManager = GetComponent<ScoreManager>();
        }
    }

    private void Start()
    {
        // Hide end-game panels when the game starts.
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        if (skipLaunchPanelOnRestart)
        {
            skipLaunchPanelOnRestart = false;

            isLaunchScreenOpen = false;
            Time.timeScale = 1f;

            if (launchPanel != null)
            {
                launchPanel.SetActive(false);
            }
        }
        else
        {
            isLaunchScreenOpen = true;
            Time.timeScale = 0f;

            if (launchPanel != null)
            {
                launchPanel.SetActive(true);
                launchPanel.transform.SetAsLastSibling();
            }
        }

        // Update UI immediately at the start.
        UpdateHUD();
    }

    private void Update()
    {
        // Do not allow pause menu while the launch screen is open.
        if (!isLaunchScreenOpen && Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // For now, update the HUD every frame.
        UpdateHUD();
    }

    /// Updates all gameplay HUD text.
    private void UpdateHUD()
    {
        UpdateHealthText();
        UpdateWaveText();
        UpdateScoreText();
        UpdateXPText();
        UpdateLevelText();
        UpdateStatsText();
        UpdateWeaponText();
    }

    /// Updates the health text using PlayerHealth values.
    private void UpdateHealthText()
    {
        if (healthText == null || playerHealth == null)
        {
            return;
        }

        healthText.text = "Health: " + playerHealth.GetCurrentHealth() + " / " + playerHealth.GetMaxHealth();
    }

    /// Updates the wave text using WaveManager values.
    private void UpdateWaveText()
    {
        if (waveText == null || waveManager == null)
        {
            return;
        }

        waveText.text = "Wave: " + waveManager.GetCurrentWaveNumber() + " / " + waveManager.GetTotalWaves();
    }

    /// Updates the score text.
    private void UpdateScoreText()
    {
        if (scoreText == null || scoreManager == null)
        {
            return;
        }

        scoreText.text = "Score: " + scoreManager.GetCurrentScore();
    }

    /// Updates the XP text using PlayerExperience values.
    private void UpdateXPText()
    {
        if (xpText == null || playerExperience == null)
        {
            return;
        }

        xpText.text = "XP: "
            + playerExperience.GetCurrentXP()
            + " / "
            + playerExperience.GetXPToNextLevel();
    }

    /// Updates the level text using PlayerExperience values.
    private void UpdateLevelText()
    {
        if (levelText == null || playerExperience == null)
        {
            return;
        }

        levelText.text = "Level: " + playerExperience.GetCurrentLevel();
    }

    /// Updates the player stats text.
    /// Shows values affected by level-up upgrades.
    private void UpdateStatsText()
    {
        if (statsText == null || playerMovement == null || playerShooting == null)
        {
            return;
        }

        statsText.text =
            "Fire Rate: " + playerShooting.GetFireRate().ToString("0.0") + "\n" +
            "Damage: " + playerShooting.GetCurrentBulletDamage() + "\n" +
            "Speed: " + playerMovement.GetMoveSpeed().ToString("0.0");
    }

    /// Updates the current weapon text.
    private void UpdateWeaponText()
    {
        if (weaponText == null || playerShooting == null)
        {
            return;
        }

        weaponText.text = "Weapon: " + playerShooting.GetCurrentWeaponName();
    }

    /// Shows the Game Over screen when the player dies.
    /// Displays final score and wave reached.
    public void ShowGameOver()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;
        isPaused = false;
        isLaunchScreenOpen = false;

        Time.timeScale = 0f;

        UpdateGameOverStats();

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
            gameOverPanel.transform.SetAsLastSibling();
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayGameOverSFX();
        }
    }

    /// Updates the final score and wave reached text on the Game Over screen.
    private void UpdateGameOverStats()
    {
        int finalScore = 0;
        int waveReached = 1;

        if (scoreManager != null)
        {
            finalScore = scoreManager.GetCurrentScore();
        }

        if (waveManager != null)
        {
            waveReached = waveManager.GetCurrentWaveNumber();
        }

        if (finalScoreText != null)
        {
            finalScoreText.text = "Final Score: " + finalScore;
        }

        if (waveReachedText != null)
        {
            waveReachedText.text = "Wave Reached: " + waveReached;
        }
    }

    /// Updates the final score and wave reached text on the Victory screen.
    private void UpdateVictoryStats()
    {
        int finalScore = 0;
        int waveReached = 1;

        if (scoreManager != null)
        {
            finalScore = scoreManager.GetCurrentScore();
        }

        if (waveManager != null)
        {
            waveReached = waveManager.GetCurrentWaveNumber();
        }

        if (victoryFinalScoreText != null)
        {
            victoryFinalScoreText.text = "Final Score: " + finalScore;
        }

        if (victoryWaveReachedText != null)
        {
            victoryWaveReachedText.text = "Wave Reached: " + waveReached;
        }
    }


    /// Shows the Victory screen when the player clears all waves.
    /// Displays final score and wave reached.
    public void ShowVictory()
    {
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;
        isPaused = false;
        isLaunchScreenOpen = false;

        Time.timeScale = 0f;

        UpdateVictoryStats();

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
            victoryPanel.transform.SetAsLastSibling();
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayVictorySFX();
        }
    }

    /// Restarts the run immediately without returning to the launch menu.
    public void RestartGame()
    {
        skipLaunchPanelOnRestart = true;

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    /// Toggles between paused and unpaused gameplay.
    public void TogglePause()
    {
        // Do not allow pausing from launch screen or after game end.
        if (gameEnded || isLaunchScreenOpen)
        {
            return;
        }

        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    /// Pauses gameplay and shows the pause menu.
    public void PauseGame()
    {
        // Do not pause if the game already ended or launch screen is open.
        if (gameEnded || isLaunchScreenOpen)
        {
            return;
        }

        isPaused = true;

        Time.timeScale = 0f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(true);
        }
    }

    /// Starts gameplay from the launch screen.
    /// Hides the launch panel and resumes game time.
    public void StartGame()
    {
        isLaunchScreenOpen = false;

        if (launchPanel != null)
        {
            launchPanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    /// Resumes gameplay and hides the pause menu.
    public void ResumeGame()
    {
        isPaused = false;

        Time.timeScale = 1f;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    /// Quits the game.
    /// This works in a built game. In the Unity Editor, it prints a debug message.
    public void QuitGame()
    {
        Debug.Log("Quit Game requested.");

        #if UNITY_EDITOR
            // Stop Play Mode when testing inside Unity Editor.
            EditorApplication.isPlaying = false;
        #else
            // Quit the actual built game.
            Application.Quit();
        #endif
    }

    /// Plays the UI button click sound.
    /// Assign this to buttons before their main action if desired.
    public void PlayButtonClick()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayButtonClickSFX();
        }
    }

    /// Reloads the scene and returns to the launch menu.
    public void ReturnToMenu()
    {
        skipLaunchPanelOnRestart = false;

        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
