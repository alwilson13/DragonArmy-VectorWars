using UnityEngine;

/// Handles random upgrade orb drops during gameplay.
/// 
/// Enemies can call this manager when they die from player damage.
/// The manager decides if an upgrade orb should drop and which type appears.
public class UpgradeDropManager : MonoBehaviour
{
    [Header("Upgrade Orb Prefabs")]

    [SerializeField] private GameObject fireRateOrbPrefab;
    [SerializeField] private GameObject bulletDamageOrbPrefab;
    [SerializeField] private GameObject moveSpeedOrbPrefab;
    [SerializeField] private GameObject maxHealthOrbPrefab;

    [Header("Drop Settings")]

    [Tooltip("Chance for any upgrade orb to drop. 0.15 = 15% chance.")]
    [Range(0f, 1f)]
    [SerializeField] private float upgradeDropChance = 0.15f;

    [Tooltip("Maximum number of upgrade orbs that can drop during one run.")]
    [SerializeField] private int maxUpgradeDropsPerRun = 6;

    private int upgradeDropsCreated;

    /// Attempts to drop one random upgrade orb at the given position.
    /// Should only be called when an enemy is killed by player bullets.
    public void TryDropUpgradeOrb(Vector3 dropPosition)
    {
        if (upgradeDropsCreated >= maxUpgradeDropsPerRun)
        {
            return;
        }

        float randomRoll = Random.value;

        if (randomRoll > upgradeDropChance)
        {
            return;
        }

        GameObject selectedOrbPrefab = GetRandomUpgradeOrbPrefab();

        if (selectedOrbPrefab == null)
        {
            Debug.LogWarning("UpgradeDropManager has no upgrade orb prefab assigned.");
            return;
        }

        Instantiate(selectedOrbPrefab, dropPosition, Quaternion.identity);

        upgradeDropsCreated++;

        Debug.Log("Upgrade orb dropped.");
    }

    /// Randomly selects one of the assigned upgrade orb prefabs.
    private GameObject GetRandomUpgradeOrbPrefab()
    {
        int randomIndex = Random.Range(0, 4);

        switch (randomIndex)
        {
            case 0:
                return fireRateOrbPrefab;

            case 1:
                return bulletDamageOrbPrefab;

            case 2:
                return moveSpeedOrbPrefab;

            case 3:
                return maxHealthOrbPrefab;

            default:
                return null;
        }
    }

    /// Resets upgrade drop count.
    /// Useful if restarting without reloading the scene.
    public void ResetDrops()
    {
        upgradeDropsCreated = 0;
    }
}