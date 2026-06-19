using UnityEngine;

/// Handles automatic upgrade orb behavior.
/// 
/// When the player touches this orb, it immediately applies
/// one upgrade without opening a level-up menu.
public class UpgradeOrb : MonoBehaviour
{
    [Header("Upgrade Settings")]

    [Tooltip("The upgrade this orb gives to the player.")]
    [SerializeField] private UpgradeType upgradeType;

    [Tooltip("How much this orb improves the selected upgrade.")]
    [SerializeField] private float upgradeAmount = 1f;

    [Tooltip("Should the orb disappear after being collected?")]
    [SerializeField] private bool destroyOnPickup = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only the player can collect upgrade orbs.
        if (!collision.CompareTag("Player"))
        {
            return;
        }

        ApplyUpgrade(collision.gameObject);

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayLevelUpSFX();
        }

        if (destroyOnPickup)
        {
            Destroy(gameObject);
        }
    }

    /// Applies the selected upgrade to the player.
    private void ApplyUpgrade(GameObject player)
    {
        PlayerShooting playerShooting = player.GetComponent<PlayerShooting>();
        PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();

        switch (upgradeType)
        {
            case UpgradeType.FireRate:
                if (playerShooting != null)
                {
                    playerShooting.IncreaseFireRate(upgradeAmount);
                    Debug.Log("Collected Fire Rate upgrade.");
                }
                break;

            case UpgradeType.BulletDamage:
                if (playerShooting != null)
                {
                    playerShooting.IncreaseBulletDamage(Mathf.RoundToInt(upgradeAmount));
                    Debug.Log("Collected Bullet Damage upgrade.");
                }
                break;

            case UpgradeType.MoveSpeed:
                if (playerMovement != null)
                {
                    playerMovement.IncreaseMoveSpeed(upgradeAmount);
                    Debug.Log("Collected Move Speed upgrade.");
                }
                break;

            case UpgradeType.MaxHealth:
                if (playerHealth != null)
                {
                    playerHealth.IncreaseMaxHealth(Mathf.RoundToInt(upgradeAmount));
                    Debug.Log("Collected Max Health upgrade.");
                }
                break;
        }
    }

    /// Allows another script to set the upgrade type after spawning the orb.
    public void SetUpgradeType(UpgradeType newUpgradeType)
    {
        upgradeType = newUpgradeType;
    }

    /// Allows another script to set the upgrade amount after spawning the orb.
    public void SetUpgradeAmount(float newAmount)
    {
        upgradeAmount = newAmount;
    }
}