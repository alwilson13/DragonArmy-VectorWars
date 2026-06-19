using UnityEngine;

/*
 * LevelUpManager.cs
 *
 * Purpose:
 * Controls the level-up menu, pauses gameplay,
 * applies player upgrades, and resumes gameplay.
 *
 * Inspiration:
 * Vampire Survivors style level-up progression systems.
 *
 * Unity References:
 * https://docs.unity3d.com/ScriptReference/Time-timeScale.html
 * https://docs.unity3d.com/ScriptReference/GameObject.SetActive.html
 *
 * Adaptations:
 * - Added custom movement speed upgrades.
 * - Added health upgrades.
 * - Added fire rate upgrades.
 * - Integrated with Vector Wars player systems.
 */

public class LevelUpManager : MonoBehaviour
{
    [Header("Menu References")]

    [Tooltip("UI menu displayed when the player levels up.")]
    [SerializeField] private GameObject levelUpMenu;

    [Header("Player References")]

    [Tooltip("Reference to the PlayerMovement component for applying upgrades.")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerShooting playerShooting;

    [Header("Upgrade Settings")]

    [Tooltip("Amount added to player movement speed when the speed upgrade is selected.")]
    [SerializeField] private float moveSpeedIncrease = 1f; 
    [SerializeField] private int maxHealthIncrease = 1;
    [SerializeField] private float fireRateIncrease = 0.05f;

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

        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<PlayerHealth>();
        }

        if (playerShooting == null)
        {
            playerShooting = FindFirstObjectByType<PlayerShooting>();
        }
    }

    /// Opens the level up menu and pauses gameplay.
    public void OpenLevelUpMenu()
    {
        Debug.Log("OpenLevelUpMenu was called.");

        if (levelUpMenu == null)
        {
            Debug.LogError("Level Up Menu reference is NULL.");
            return;
        }
        else
        {
            Debug.Log("Level Up Menu reference is: " + levelUpMenu.name);
        }

        isLevelUpMenuOpen = true;

        Time.timeScale = 0f;

        levelUpMenu.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
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

    public void SelectHealthUpgrade()
    {
        if (playerHealth != null)
        {
            playerHealth.IncreaseMaxHealth(maxHealthIncrease);
        }
        else
        {
            Debug.LogWarning("LevelUpManager could not find PlayerHealth.");
        }

        CloseLevelUpMenu();
    }

    public void SelectFireRateUpgrade()
    {
        if (playerShooting != null)
        {
            playerShooting.IncreaseFireRate(fireRateIncrease);
        }
        else
        {
            Debug.LogWarning("LevelUpManager could not find PlayerShooting.");
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

        Time.timeScale = 1f;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    /// Returns whether the level up menu is currently open.
    public bool IsLevelUpMenuOpen()
    {
        return isLevelUpMenuOpen;
    }
}