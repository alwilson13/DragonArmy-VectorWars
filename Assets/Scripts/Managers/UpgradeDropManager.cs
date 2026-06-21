using System.Collections.Generic;
using UnityEngine;

/// Handles random upgrade orb drops during gameplay.
/// 
/// Each upgrade type can be enabled or disabled from the Inspector.
public class UpgradeDropManager : MonoBehaviour
{
    [Header("Upgrade Orb Prefabs")]

    [SerializeField] private GameObject fireRateOrbPrefab;
    [SerializeField] private GameObject bulletDamageOrbPrefab;
    [SerializeField] private GameObject moveSpeedOrbPrefab;
    [SerializeField] private GameObject maxHealthOrbPrefab;

    [Header("Enabled Upgrade Orbs")]

    [SerializeField] private bool enableFireRateOrb = true;
    [SerializeField] private bool enableBulletDamageOrb = true;
    [SerializeField] private bool enableMoveSpeedOrb = true;
    [SerializeField] private bool enableMaxHealthOrb = true;

    [Header("Drop Settings")]

    [Tooltip("Chance for any upgrade orb to drop. 0.15 = 15% chance.")]
    [Range(0f, 1f)]
    [SerializeField] private float upgradeDropChance = 0.15f;

    [Tooltip("Maximum number of upgrade orbs that can drop during one run.")]
    [SerializeField] private int maxUpgradeDropsPerRun = 6;

    private int upgradeDropsCreated;

    /// Attempts to drop one random enabled upgrade orb at the given position.
    /// Should only be called when an enemy is killed by player bullets.
    public void TryDropUpgradeOrb(Vector3 dropPosition)
    {
        if (upgradeDropsCreated >= maxUpgradeDropsPerRun)
        {
            return;
        }

        if (Random.value > upgradeDropChance)
        {
            return;
        }

        GameObject selectedOrbPrefab = GetRandomEnabledUpgradeOrbPrefab();

        if (selectedOrbPrefab == null)
        {
            Debug.LogWarning("No enabled upgrade orb prefab is available.");
            return;
        }

        Instantiate(selectedOrbPrefab, dropPosition, Quaternion.identity);

        upgradeDropsCreated++;

        Debug.Log("Upgrade orb dropped.");
    }

    /// Builds a list of enabled upgrade orb prefabs, then returns one randomly.
    private GameObject GetRandomEnabledUpgradeOrbPrefab()
    {
        List<GameObject> availableOrbs = new List<GameObject>();

        if (enableFireRateOrb && fireRateOrbPrefab != null)
        {
            availableOrbs.Add(fireRateOrbPrefab);
        }

        if (enableBulletDamageOrb && bulletDamageOrbPrefab != null)
        {
            availableOrbs.Add(bulletDamageOrbPrefab);
        }

        if (enableMoveSpeedOrb && moveSpeedOrbPrefab != null)
        {
            availableOrbs.Add(moveSpeedOrbPrefab);
        }

        if (enableMaxHealthOrb && maxHealthOrbPrefab != null)
        {
            availableOrbs.Add(maxHealthOrbPrefab);
        }

        if (availableOrbs.Count == 0)
        {
            return null;
        }

        int randomIndex = Random.Range(0, availableOrbs.Count);

        return availableOrbs[randomIndex];
    }

    /// Resets upgrade drop count.
    /// Useful if restarting without reloading the scene.
    public void ResetDrops()
    {
        upgradeDropsCreated = 0;
    }
}