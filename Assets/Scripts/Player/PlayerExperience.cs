using UnityEngine;

/// Handles the player's experience and level values.
/// 
/// XP is collected from XP orbs dropped by enemies.
/// When the player reaches enough XP, they level up and the Level-Up menu opens.
public class PlayerExperience : MonoBehaviour
{
    [Header("Experience Settings")]

    [Tooltip("The player's current level.")]
    [SerializeField] private int currentLevel = 1;

    [Tooltip("The player's current XP amount.")]
    [SerializeField] private int currentXP = 0;

    [Tooltip("How much XP the player needs to reach the next level.")]
    [SerializeField] private int xpToNextLevel = 100;

    [Tooltip("How much harder each level becomes. Example: 1.25 = 25% more XP required.")]
    [SerializeField] private float xpRequirementMultiplier = 1.25f;

    [Header("References")]

    [Tooltip("Reference to the LevelUpManager.")]
    [SerializeField] private LevelUpManager levelUpManager;

    private void Awake()
    {
        // If LevelUpManager was not assigned, find it in the scene.
        if (levelUpManager == null)
        {
            levelUpManager = FindFirstObjectByType<LevelUpManager>();
        }
    }

    /// Adds XP to the player and checks for level-up.
    public void AddXP(int xpAmount)
    {
        if (xpAmount <= 0)
        {
            return;
        }

        currentXP += xpAmount;

        Debug.Log("Player gained " + xpAmount + " XP. Current XP: " + currentXP);

        CheckForLevelUp();
    }

    /// Checks whether the player has enough XP to level up.
    /// Supports XP carry-over if the player has extra XP.
    private void CheckForLevelUp()
    {
        if (currentXP < xpToNextLevel)
        {
            return;
        }

        currentXP -= xpToNextLevel;
        currentLevel++;

        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpRequirementMultiplier);

        Debug.Log("Player leveled up to level " + currentLevel);

        if (levelUpManager != null)
        {
            levelUpManager.OpenLevelUpMenu();
        }
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

    /// Returns the XP required for the next level.
    public int GetXPToNextLevel()
    {
        return xpToNextLevel;
    }
}