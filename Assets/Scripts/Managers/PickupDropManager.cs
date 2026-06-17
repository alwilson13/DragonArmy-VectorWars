using UnityEngine;

/// Handles special pickup drops during gameplay.
/// 
/// For now, this manager controls weapon pickup drops from defeated enemies.
/// It helps prevent too many weapon pickups from appearing at once.
public class PickupDropManager : MonoBehaviour
{
    [Header("Weapon Pickup Drop Settings")]

    [Tooltip("The weapon pickup prefab that can drop from enemies.")]
    [SerializeField] private GameObject weaponPickupPrefab;

    [Tooltip("Chance for a weapon pickup to drop when an enemy is killed by bullets. 0.15 = 15% chance.")]
    [Range(0f, 1f)]
    [SerializeField] private float weaponDropChance = 0.15f;

    [Tooltip("Maximum number of weapon pickups that can drop during one run.")]
    [SerializeField] private int maxWeaponDropsPerRun = 1;

    // Tracks how many weapon pickups have dropped so far.
    private int weaponDropsCreated;

    /// Attempts to drop a weapon pickup at a specific position.
    /// This should only be called when an enemy is defeated by the player.
    public void TryDropWeaponPickup(Vector3 dropPosition)
    {
        // Do not drop if no prefab is assigned.
        if (weaponPickupPrefab == null)
        {
            return;
        }

        // Do not drop more than the max allowed for the run.
        if (weaponDropsCreated >= maxWeaponDropsPerRun)
        {
            return;
        }

        // Roll a random number between 0 and 1.
        float randomRoll = Random.value;

        // If the roll is higher than the drop chance, no pickup drops.
        if (randomRoll > weaponDropChance)
        {
            return;
        }

        // Spawn the weapon pickup at the enemy's death position.
        Instantiate(
            weaponPickupPrefab,
            dropPosition,
            Quaternion.identity
        );

        weaponDropsCreated++;

        Debug.Log("Weapon pickup dropped.");
    }

    /// Resets weapon drop count.
    /// Useful if we later restart runs without reloading the scene.
    public void ResetDrops()
    {
        weaponDropsCreated = 0;
    }
}