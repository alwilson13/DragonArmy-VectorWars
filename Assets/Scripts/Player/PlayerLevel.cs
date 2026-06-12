using UnityEngine;

/// Tracks player experience, level progression, and XP requirements.
///
/// The PlayerLevel system is responsible for:
/// - Storing the player's current level.
/// - Tracking current XP.
/// - Calculating XP requirements for future levels.
/// - Triggering the level up menu when a level is gained.
///
/// XP requirements increase as the player's level increases.
/// Additional scaling is applied at level intervals to slow progression
/// during longer runs.
public class PlayerLevel : MonoBehaviour
{
    [Header("Level Stats")]

    [Tooltip("The player's current level.")]
    [SerializeField] private int currentLevel = 1;

    [Tooltip("The player's current XP toward the next level.")]
    [SerializeField] private int currentXP = 0;

    [Tooltip("XP required to reach the next level.")]
    [SerializeField] private int xpToNextLevel = 20;

    [Header("XP Scaling")]

    [Tooltip("Base XP requirement used for level calculations.")]
    [SerializeField] private int baseXPRequirement = 20;

    [Tooltip("Additional XP added per level.")]
    [SerializeField] private int xpIncreasePerLevel = 10;

    [Tooltip("How many levels pass before an XP multiplier is applied.")]
    [SerializeField] private int levelInterval = 20;

    [Tooltip("Multiplier applied each level interval to increase progression difficulty.")]
    [SerializeField] private float intervalMultiplierIncrease = 1.5f;

    [Header("References")]

    [Tooltip("Reference to the LevelUpManager used to display upgrades.")]
    [SerializeField] private LevelUpManager levelUpManager;

    private void Awake()
    {
        // Automatically locate the LevelUpManager if one was not assigned.
        if (levelUpManager == null)
        {
            levelUpManager = FindFirstObjectByType<LevelUpManager>();
        }

        // Calculate the XP requirement for the starting level.
        xpToNextLevel = CalculateXPRequirement(currentLevel);
    }

    /// Adds XP to the player and checks for level ups.
    public void GainXP(int amount)
    {
        // Ignore invalid XP values.
        if (amount <= 0)
        {
            return;
        }

        currentXP += amount;

        // Continue leveling up while enough XP exists.
        // This allows large XP gains to award multiple levels.
        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }

        Debug.Log("XP: " + currentXP + " / " + xpToNextLevel);
    }

    /// Forces an immediate level up.
    /// Used primarily for development and testing.
    public void ForceLevelUp()
    {
        currentXP = 0;
        LevelUp();
    }

    /// Increases the player's level and opens the level up menu.
    private void LevelUp()
    {
        currentLevel++;

        // Recalculate the XP requirement for the next level.
        xpToNextLevel = CalculateXPRequirement(currentLevel);

        Debug.Log("Level Up! Current Level: " + currentLevel);

        if (levelUpManager != null)
        {
            levelUpManager.OpenLevelUpMenu();
        }
    }

    /// Calculates how much XP is needed for the specified level.
    ///
    /// Formula:
    /// Base XP + (Level * XP Increase)
    ///
    /// Every level interval, an additional multiplier is applied
    /// to slow progression in later stages.
    private int CalculateXPRequirement(int level)
    {
        int intervalCount = (level - 1) / levelInterval;

        float multiplier = Mathf.Pow(intervalMultiplierIncrease, intervalCount);

        int requirement = baseXPRequirement + (level * xpIncreasePerLevel);

        return Mathf.RoundToInt(requirement * multiplier);
    }

    /// Returns the player's current level.
    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    /// Returns the player's current XP.
    public int GetCurrentXP()
    {
        return currentXP;
    }

    /// Returns the XP required to reach the next level.
    public int GetXPToNextLevel()
    {
        return xpToNextLevel;
    }

    /// Returns current XP progress as a percentage.
    /// Intended for XP bar UI updates.
    public float GetXPPercent()
    {
        return (float)currentXP / xpToNextLevel;
    }
}