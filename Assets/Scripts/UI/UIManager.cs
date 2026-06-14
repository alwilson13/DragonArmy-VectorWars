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

    [Header("Panels")]

    [Tooltip("Panel shown when the player dies.")]
    [SerializeField] private GameObject gameOverPanel;

    [Tooltip("Panel shown when the player clears all waves.")]
    [SerializeField] private GameObject victoryPanel;

    [Tooltip("Panel shown when the player pauses the game.")]
    [SerializeField] private GameObject pausePanel;

    [Header("References")]

    [Tooltip("Reference to the player's health script.")]
    [SerializeField] private PlayerHealth playerHealth;

    [Tooltip("Reference to the WaveManager script.")]
    [SerializeField] private WaveManager waveManager;

    [Tooltip("Reference to the ScoreManager script.")]
    [SerializeField] private ScoreManager scoreManager;

    [Tooltip("Reference to the PlayerExperience script.")]
    [SerializeField] private PlayerExperience playerExperience;


    // Prevents Game Over and Victory from showing at the same time.
    private bool gameEnded;

    // Tracks whether the game is currently paused.
    private bool isPaused;

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

        // Make sure time is normal when the scene starts.
        Time.timeScale = 1f;

        // Update UI immediately at the start.
        UpdateHUD();
    }

    private void Update()
    {
        // Press Escape to pause or resume the game.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }

        // For now, update the HUD every frame.
        // Later, we can optimize this using events.
        UpdateHUD();
    }

    /// Updates health, wave, and score text on the screen.
    private void UpdateHUD()
    {
        UpdateHealthText();
        UpdateWaveText();
        UpdateScoreText();
        UpdateXPText();
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

    /// Shows the Game Over panel.
    /// Called when the player dies.
    public void ShowGameOver()
    {
        // If Victory or Game Over already happened, do nothing.
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(false);
        }
    }


    /// Shows the Victory panel.
    /// Called when all waves are cleared.
    public void ShowVictory()
    {
        // If Victory or Game Over already happened, do nothing.
        if (gameEnded)
        {
            return;
        }

        gameEnded = true;

        if (victoryPanel != null)
        {
            victoryPanel.SetActive(true);
        }

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    /// Restarts the current scene.
    /// This is used by the restart buttons.
    public void RestartGame()
    {
        // Reset time in case the game was paused.
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// Toggles between paused and unpaused gameplay.
    /// This is called when the player presses Escape.
    public void TogglePause()
    {
        // Do not allow pausing after Game Over or Victory.
        if (gameEnded)
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
    /// Time.timeScale = 0 freezes physics, movement, enemies, and wave timing.
    public void PauseGame()
    {
        // Do not pause if the game already ended.
        if (gameEnded)
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
}
