using UnityEngine;

/// Controls the level up menu and upgrade selection process.
///
/// The LevelUpManager is responsible for:
/// - Opening the level up menu when the player gains a level.
/// - Pausing gameplay while the player selects an upgrade.
/// - Applying the selected upgrade.
/// - Resuming gameplay after a selection is made.
///
/// Currently implemented upgrades:
/// - Movement Speed Increase
///
/// Additional upgrades can be added later as the progression system expands.
public class LevelUpManager : MonoBehaviour
{
    [Header("Menu References")]

    [Tooltip("UI menu displayed when the player levels up.")]
    [SerializeField] private GameObject levelUpMenu;

    [Header("Player References")]

    [Tooltip("Reference to the PlayerMovement component for applying upgrades.")]
    [SerializeField] private PlayerMovement playerMovement;

    [Header("Upgrade Settings")]

    [Tooltip("Amount added to player movement speed when the speed upgrade is selected.")]
    [SerializeField] private float moveSpeedIncrease = 1f;

    // Tracks whether the level up menu is currently open.
    private bool isLevelUpMenuOpen;

    private void Awake()
    {
        // Hide the menu when the scene starts.
        if (levelUpMenu != null)
        {
            levelUpMenu.SetActive(false);
        }

        // Automatically find the player movement component if one was not assigned.
        if (playerMovement == null)
        {
            playerMovement = FindFirstObjectByType<PlayerMovement>();
        }
    }

    /// Opens the level up menu and pauses gameplay.
    public void OpenLevelUpMenu()
    {
        if (levelUpMenu == null)
        {
            Debug.LogWarning("LevelUpManager is missing the Level Up Menu reference.");
            return;
        }

        isLevelUpMenuOpen = true;

        // Pause gameplay while the player chooses an upgrade.
        Time.timeScale = 0f;

        levelUpMenu.SetActive(true);
    }

    /// Applies the movement speed upgrade and closes the menu.
    public void SelectMoveSpeedUpgrade()
    {
        if (playerMovement != null)
        {
            playerMovement.IncreaseMoveSpeed(moveSpeedIncrease);

            Debug.Log("Move Speed increased by " + moveSpeedIncrease);
        }
        else
        {
            Debug.LogWarning("LevelUpManager could not find PlayerMovement.");
        }

        CloseLevelUpMenu();
    }

    /// Closes the level up menu and resumes gameplay.
    public void CloseLevelUpMenu()
    {
        isLevelUpMenuOpen = false;

        if (levelUpMenu != null)
        {
            levelUpMenu.SetActive(false);
        }

        // Resume gameplay after an upgrade is selected.
        Time.timeScale = 1f;
    }

    /// Returns whether the level up menu is currently open.
    public bool IsLevelUpMenuOpen()
    {
        return isLevelUpMenuOpen;
    }
}