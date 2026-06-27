using UnityEngine;

/// Tracks player XP and level.
/// Leveling up now happens automatically without opening a menu.
public class PlayerExperience : MonoBehaviour
{
    [Header("Experience Settings")]
    [SerializeField] private int currentLevel = 1;
    [SerializeField] private int currentXP = 0;
    [SerializeField] private int xpToNextLevel = 200;
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
        // Play level-up sound.
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLevelUpSFX();
        }

        PlayerHealth playerHealth = GetComponent<PlayerHealth>();
        PlayerMovement playerMovement = GetComponent<PlayerMovement>();
        PlayerShooting playerShooting = GetComponent<PlayerShooting>();

        UIManager uiManager = FindFirstObjectByType<UIManager>();

        // If player is damaged, heal them.
        if (playerHealth != null && !playerHealth.IsFullHealth())
        {
            playerHealth.Heal(healthReward);
        }
        else
        {
            // If player is already full health, give a small stat upgrade instead.
            GiveFullHealthLevelUpBonus(playerMovement, playerShooting, uiManager);
        }

        // Add score bonus.
        ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>();

        if (scoreManager != null)
        {
            int scoreBonus = currentLevel * scoreBonusPerLevel;
            scoreManager.AddScore(scoreBonus);

            Debug.Log("Level up score bonus added: " + scoreBonus);
        }

        // Show level-up popup.
        if (uiManager != null)
        {
            uiManager.ShowLevelUpMessage(currentLevel);
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

    /// Gives a small bonus when the player levels up while already at full health.
    /// Randomly upgrades either move speed or bullet damage.
    private void GiveFullHealthLevelUpBonus(
        PlayerMovement playerMovement,
        PlayerShooting playerShooting,
        UIManager uiManager)
    {
        int randomBonus = Random.Range(0, 2);

        if (randomBonus == 0 && playerMovement != null)
        {
            float speedBonus = 0.25f;

            playerMovement.IncreaseMoveSpeed(speedBonus);

            if (uiManager != null)
            {
                uiManager.ShowSpeedPopup(speedBonus);
            }

            Debug.Log("Full health level-up bonus: +" + speedBonus + " Speed");
        }
        else if (playerShooting != null)
        {
            int damageBonus = 1;

            playerShooting.IncreaseBulletDamage(damageBonus);

            if (uiManager != null)
            {
                uiManager.ShowDamagePopup(damageBonus);
            }

            Debug.Log("Full health level-up bonus: +" + damageBonus + " Damage");
        }
    }
}