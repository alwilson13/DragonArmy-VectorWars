using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    [Header("Panels")]

    [Tooltip("Panel shown when the player dies.")]
    [SerializeField] private GameObject gameOverPanel;

    [Tooltip("Panel shown when the player clears all waves.")]
    [SerializeField] private GameObject victoryPanel;

    [Header("References")]

    [Tooltip("Reference to the player's health script.")]
    [SerializeField] private PlayerHealth playerHealth;

    [Tooltip("Reference to the WaveManager script.")]
    [SerializeField] private WaveManager waveManager;

    private int score = 0;

    // Prevents Game Over and Victory from showing at the same time.
    private bool gameEnded;

    private void Awake()
    {
        // If PlayerHealth is not assigned, try to find the player by tag.
        if (playerHealth == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                playerHealth = playerObject.GetComponent<PlayerHealth>();
            }
        }

        // If WaveManager is not assigned, try to find it on the same GameObject.
        if (waveManager == null)
        {
            waveManager = GetComponent<WaveManager>();
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

        // Update UI immediately at the start.
        UpdateHUD();
    }

    private void Update()
    {
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
        if (scoreText == null)
        {
            return;
        }

        scoreText.text = "Score: " + score;
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// Placeholder score setter.
    public void SetScore(int newScore)
    {
        score = newScore;
    }
}