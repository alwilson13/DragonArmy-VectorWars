using UnityEngine;

/// Handles player health, damage, temporary invincibility, and death.
/// 
/// The player loses health when touched by enemies.
/// When health reaches zero, the player dies.
public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]

    [Tooltip("The player's maximum health.")]
    [SerializeField] private int maxHealth = 5;

    [Tooltip("How long the player is invincible after taking damage.")]
    [SerializeField] private float invincibilityDuration = 1f;

    // Current player health during gameplay.
    private int currentHealth;

    // Tracks whether the player is temporarily invincible.
    private bool isInvincible;

    // Tracks whether the player is already dead.
    private bool isDead;

    private void Awake()
    {
        // Start the player with full health.
        currentHealth = maxHealth;
    }

    /// Damages the player by a specific amount.
    public void TakeDamage(int damageAmount)
    {
        // Do not take damage if already dead or temporarily invincible.
        if (isDead || isInvincible)
        {
            return;
        }

        // Subtract damage from health.
        currentHealth -= damageAmount;

        Debug.Log("Player took " + damageAmount + " damage. Health left: " + currentHealth);

        // Check if the player should die.
        if (currentHealth <= 0)
        {
            Die();
            return;
        }

        // Start temporary invincibility after taking damage.
        StartCoroutine(InvincibilityRoutine());
    }

    /// Gives the player temporary invincibility after being hit.
    /// This prevents the player from losing all health instantly while touching an enemy.
    private System.Collections.IEnumerator InvincibilityRoutine()
    {
        isInvincible = true;

        // Optional visual feedback: make the player blink.
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();

        float blinkTimer = 0f;
        float blinkInterval = 0.1f;

        while (blinkTimer < invincibilityDuration)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }

            yield return new WaitForSeconds(blinkInterval);
            blinkTimer += blinkInterval;
        }

        // Make sure the player is visible again after blinking.
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
        }

        isInvincible = false;
    }

    /// Handles player death.
    /// For now, this disables movement and shooting and prints a Game Over message.
    private void Die()
    {
        // Prevent this function from running more than once.
        if (isDead)
        {
            return;
        }

        isDead = true;

        Debug.Log("Game Over! Player died.");

        // Show the Game Over UI screen.
        UIManager uiManager = FindFirstObjectByType<UIManager>();

        if (uiManager != null)
        {
            uiManager.ShowGameOver();
        }

        // Stop the wave system after the player dies.
        // This prevents the game from triggering Victory after Game Over.
        WaveManager waveManager = FindFirstObjectByType<WaveManager>();

        if (waveManager != null)
        {
            waveManager.enabled = false;
        }

        // Stop player movement.
        PlayerMovement movement = GetComponent<PlayerMovement>();

        if (movement != null)
        {
            movement.enabled = false;
        }

        // Stop player aiming.
        PlayerAiming aiming = GetComponent<PlayerAiming>();

        if (aiming != null)
        {
            aiming.enabled = false;
        }

        // Stop player shooting.
        PlayerShooting shooting = GetComponent<PlayerShooting>();

        if (shooting != null)
        {
            shooting.enabled = false;
        }

        // Stop Rigidbody2D movement.
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    /// Returns the player's current health.
    /// This will be useful later for updating the UI.
    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    /// Returns the player's maximum health.
    /// This will be useful later for health bars or heart displays.
    public int GetMaxHealth()
    {
        return maxHealth;
    }

    /// Returns whether the player is dead.
    public bool IsDead()
    {
        return isDead;
    }

    /// Increases the player's maximum health and also heals them by the same amount.
    /// Used by level-up upgrades.
    public void IncreaseMaxHealth(int amount)
    {
        maxHealth += amount;
        currentHealth += amount;

        Debug.Log("Max health increased to: " + maxHealth);
    }

    /// Temporarily sets player invincibility from another script.
    /// Used by the dodge ability so the player can avoid damage while dashing.
    public void SetInvincible(bool value)
    {
        isInvincible = value;
    }
}