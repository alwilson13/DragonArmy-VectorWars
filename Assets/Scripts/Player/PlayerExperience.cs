using UnityEngine;

/// Tracks player XP and level.
/// Leveling up now happens automatically without opening a menu.
public class PlayerExperience : MonoBehaviour
{
    [Header("Experience Settings")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int xpToNextLevel = 100;
    [SerializeField] private float xpRequirementMultiplier = 1.25f;

    [Header("Level Up Rewards")]
    [SerializeField] private int healthReward = 1;
    [SerializeField] private int scoreBonusPerLevel = 250;

    /// Adds XP to the player.
    /// Called by XP orbs.
    public void AddXP(int amount)
    {
        currentXP += amount;

        Debug.Log("XP gained: " + amount + ". Current XP: " + currentXP + " / " + xpToNextLevel);

        CheckForLevelUp();
    }

    /// Checks if the player has enough XP to level up.
    private void CheckForLevelUp()
    {
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            currentLevel++;

            xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpRequirementMultiplier);

            Debug.Log("LEVEL UP! New level: " + currentLevel);

            ApplyLevelUpRewards();
        }
    }

    /// Gives automatic rewards whenever the player levels up.
    private void ApplyLevelUpRewards()
    {
        // Play sound.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLevelUpSFX();
        }

        // Heal player.
        PlayerHealth playerHealth = GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.Heal(healthReward);
        }
        else
        {
            Debug.LogWarning("PlayerExperience could not find PlayerHealth on the player.");
        }

        // Add score bonus.
        ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>();

        if (scoreManager != null)
        {
            int scoreBonus = currentLevel * scoreBonusPerLevel;
            scoreManager.AddScore(scoreBonus);
            Debug.Log("Level up score bonus added: " + scoreBonus);
        }
        else
        {
            Debug.LogWarning("PlayerExperience could not find ScoreManager.");
        }

        // Show UI message.
        UIManager uiManager = FindFirstObjectByType<UIManager>();

        if (uiManager != null)
        {
            uiManager.ShowLevelUpMessage(currentLevel);
        }
        else
        {
            Debug.LogWarning("PlayerExperience could not find UIManager.");
        }
    }

    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    public int GetCurrentXP()
    {
        return currentXP;
    }

    public int GetXPToNextLevel()
    {
        return xpToNextLevel;
    }
}