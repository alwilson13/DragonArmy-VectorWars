using UnityEngine;

/// Handles weapon pickup behavior.
/// 
/// When the player touches this pickup, the player's weapon changes
/// to the weapon type stored on this object.
public class WeaponPickup : MonoBehaviour
{
    [Header("Weapon Pickup Settings")]

    [Tooltip("The weapon type this pickup gives to the player.")]
    [SerializeField] private WeaponType weaponType = WeaponType.Spread;

    [Tooltip("Should this pickup disappear after being collected?")]
    [SerializeField] private bool destroyOnPickup = true;

    /// Called when another collider enters this pickup's trigger.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only the player can collect weapon pickups.
        if (collision.CompareTag("Player"))
        {
            PlayerShooting playerShooting = collision.GetComponent<PlayerShooting>();

            if (playerShooting != null)
            {
                playerShooting.SetWeapon(weaponType);
            }

            if (destroyOnPickup)
            {
                Destroy(gameObject);
            }
        }
    }
}