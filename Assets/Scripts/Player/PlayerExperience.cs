using UnityEngine;

/// Handles the player's experience and level values.
/// 
/// XP is collected from XP orbs dropped by enemies.
public class PlayerExperience : MonoBehaviour
{
    [Header("Experience Settings")]

    [Tooltip("The player's current level.")]
    [SerializeField] private int currentLevel = 1;

    [Tooltip("The player's current XP amount.")]
    [SerializeField] private int currentXP = 0;

    [Tooltip("How much XP the player needs to reach the next level.")]
    [SerializeField] private int xpToNextLevel = 100;

    /// Adds XP to the player.
    public void AddXP(int xpAmount)
    {
        // Ignore invalid XP values.
        if (xpAmount <= 0)
        {
            return;
        }

        currentXP += xpAmount;

        Debug.Log("Player gained " + xpAmount + " XP. Current XP: " + currentXP);

        if (currentXP >= xpToNextLevel)
        {
            Debug.Log("Player has enough XP to level up.");
        }
    }

    /// Returns the player's current level.
    /// Used by UI scripts.
    public int GetCurrentLevel()
    {
        return currentLevel;
    }

    /// Returns the player's current XP.
    /// Used by UI scripts.
    public int GetCurrentXP()
    {
        return currentXP;
    }

    /// Returns the XP required for the next level.
    /// Used by UI scripts.
    public int GetXPToNextLevel()
    {
        return xpToNextLevel;
    }
}