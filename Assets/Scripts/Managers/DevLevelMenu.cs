using UnityEngine;

/// Temporary developer menu used to test the level progression system.
///
/// This menu allows developers to manually add XP and trigger level ups
/// without needing enemy XP drops, pickups, or wave progression.
///
/// These functions are intended to be connected to UI Buttons in the scene
/// and may be removed or disabled once the XP drop system is complete.
public class DevLevelMenu : MonoBehaviour
{
    [Header("References")]

    [Tooltip("Reference to the PlayerLevel component used for testing XP gain.")]
    [SerializeField] private PlayerLevel playerLevel;

    private void Awake()
    {
        // If no PlayerLevel was assigned in the Inspector,
        // automatically find one in the scene.
        if (playerLevel == null)
        {
            playerLevel = FindFirstObjectByType<PlayerLevel>();
        }
    }

    /// Adds 1 XP to the player.
    /// Useful for testing small XP gains.
    public void AddOneXP()
    {
        playerLevel.GainXP(1);
    }

    /// Adds 5 XP to the player.
    /// Useful for testing early level progression.
    public void AddFiveXP()
    {
        playerLevel.GainXP(5);
    }

    /// Adds 25 XP to the player.
    /// Useful for quickly reaching level thresholds.
    public void AddTwentyFiveXP()
    {
        playerLevel.GainXP(25);
    }

    /// Adds 100 XP to the player.
    /// Useful for testing multiple level gains.
    public void AddOneHundredXP()
    {
        playerLevel.GainXP(100);
    }

    /// Forces an immediate level up.
    /// Bypasses normal XP requirements for testing purposes.
    public void ForceLevelUp()
    {
        playerLevel.ForceLevelUp();
    }
}