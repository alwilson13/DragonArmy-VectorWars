using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// Handles the level-up menu and upgrade selection.
/// 
/// When the player gains enough XP, this manager pauses the game,
/// shows three upgrade choices, applies the selected upgrade,
/// then resumes gameplay.
public class LevelUpManager : MonoBehaviour
{
    private enum UpgradeType
    {
        FireRate,
        BulletDamage,
        MoveSpeed,
        MaxHealth
    }

    [System.Serializable]
    private class UpgradeOption
    {
        public string upgradeName;
        public string description;
        public UpgradeType upgradeType;
    }

    [Header("UI References")]

    [Tooltip("The panel shown when the player levels up.")]
    [SerializeField] private GameObject levelUpPanel;

    [Tooltip("The three upgrade buttons shown to the player.")]
    [SerializeField] private Button[] upgradeButtons;

    [Tooltip("The text labels inside the upgrade buttons.")]
    [SerializeField] private TMP_Text[] upgradeButtonTexts;

    [Header("Player References")]

    [Tooltip("Reference to player movement for speed upgrades.")]
    [SerializeField] private PlayerMovement playerMovement;

    [Tooltip("Reference to player shooting for fire rate and damage upgrades.")]
    [SerializeField] private PlayerShooting playerShooting;

    [Tooltip("Reference to player health for max health upgrades.")]
    [SerializeField] private PlayerHealth playerHealth;

    // The current upgrade options being displayed.
    private UpgradeOption[] currentOptions;

    // Tracks whether the level-up menu is currently open.
    private bool isLevelUpOpen;

    private void Awake()
    {
        // Automatically find the player if references were not assigned.
        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            if (playerMovement == null)
            {
                playerMovement = playerObject.GetComponent<PlayerMovement>();
            }

            if (playerShooting == null)
            {
                playerShooting = playerObject.GetComponent<PlayerShooting>();
            }

            if (playerHealth == null)
            {
                playerHealth = playerObject.GetComponent<PlayerHealth>();
            }
        }
    }

    private void Start()
    {
        // Hide the level-up panel when the game starts.
        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
        }

        SetupButtonEvents();
    }

    /// Connects each upgrade button to its selection function.
    private void SetupButtonEvents()
    {
        for (int i = 0; i < upgradeButtons.Length; i++)
        {
            int buttonIndex = i;

            if (upgradeButtons[i] != null)
            {
                upgradeButtons[i].onClick.RemoveAllListeners();
                upgradeButtons[i].onClick.AddListener(() => SelectUpgrade(buttonIndex));
            }
        }
    }

    /// Opens the level-up menu and pauses gameplay.
    public void OpenLevelUpMenu()
    {
        if (isLevelUpOpen)
        {
            return;
        }

        isLevelUpOpen = true;

        GenerateUpgradeOptions();
        UpdateUpgradeButtonText();

        Time.timeScale = 0f;

        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(true);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLevelUpSFX();
        }
    }

    /// Creates three unique upgrade choices.
    private void GenerateUpgradeOptions()
    {
        // Create the full upgrade pool.
        UpgradeOption[] upgradePool =
        {
        new UpgradeOption
        {
            upgradeName = "Fire Rate Up",
            description = "Shoot faster.",
            upgradeType = UpgradeType.FireRate
        },
        new UpgradeOption
        {
            upgradeName = "Damage Up",
            description = "Bullets deal more damage.",
            upgradeType = UpgradeType.BulletDamage
        },
        new UpgradeOption
        {
            upgradeName = "Speed Up",
            description = "Move faster.",
            upgradeType = UpgradeType.MoveSpeed
        },
        new UpgradeOption
        {
            upgradeName = "Max Health Up",
            description = "Gain more max health.",
            upgradeType = UpgradeType.MaxHealth
        }
    };

        // Create a temporary list so we can remove options after choosing them.
        System.Collections.Generic.List<UpgradeOption> availableOptions =
            new System.Collections.Generic.List<UpgradeOption>(upgradePool);

        currentOptions = new UpgradeOption[3];

        for (int i = 0; i < currentOptions.Length; i++)
        {
            // Pick a random option from the remaining available options.
            int randomIndex = Random.Range(0, availableOptions.Count);

            currentOptions[i] = availableOptions[randomIndex];

            // Remove the selected option so it cannot appear again this level-up.
            availableOptions.RemoveAt(randomIndex);
        }
    }

    /// Updates the button labels with the current upgrade options.
    private void UpdateUpgradeButtonText()
    {
        for (int i = 0; i < upgradeButtonTexts.Length; i++)
        {
            if (upgradeButtonTexts[i] != null && currentOptions != null && i < currentOptions.Length)
            {
                upgradeButtonTexts[i].text =
                    currentOptions[i].upgradeName +
                    "\n\n" +
                    currentOptions[i].description;
            }
        }
    }

    /// Applies the selected upgrade and closes the level-up menu.
    private void SelectUpgrade(int optionIndex)
    {
        if (currentOptions == null || optionIndex < 0 || optionIndex >= currentOptions.Length)
        {
            return;
        }

        ApplyUpgrade(currentOptions[optionIndex]);
        CloseLevelUpMenu();
    }

    /// Applies a specific upgrade to the player.
    private void ApplyUpgrade(UpgradeOption option)
    {
        switch (option.upgradeType)
        {
            case UpgradeType.FireRate:
                if (playerShooting != null)
                {
                    playerShooting.IncreaseFireRate(1f);
                }
                break;

            case UpgradeType.BulletDamage:
                if (playerShooting != null)
                {
                    playerShooting.IncreaseBulletDamage(1);
                }
                break;

            case UpgradeType.MoveSpeed:
                if (playerMovement != null)
                {
                    playerMovement.IncreaseMoveSpeed(0.75f);
                }
                break;

            case UpgradeType.MaxHealth:
                if (playerHealth != null)
                {
                    playerHealth.IncreaseMaxHealth(1);
                }
                break;
        }

        Debug.Log("Selected upgrade: " + option.upgradeName);
    }

    /// Closes the level-up menu and resumes gameplay.
    private void CloseLevelUpMenu()
    {
        isLevelUpOpen = false;

        if (levelUpPanel != null)
        {
            levelUpPanel.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    /// Returns whether the level-up menu is open.
    /// Useful for other systems later.
    public bool IsLevelUpOpen()
    {
        return isLevelUpOpen;
    }
}