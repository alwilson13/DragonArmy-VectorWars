using UnityEngine;

/// Handles basic player shooting for Vector wars.
/// 
/// The player fires bullets from the FirePoint.
/// Bullets travel in the direction the player is currently facing.
public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting References")]

    [Tooltip("The bullet prefab that will be spawned when the player shoots.")]
    [SerializeField] private GameObject bulletPrefab;

    [Tooltip("The position where bullets will spawn.")]
    [SerializeField] private Transform firePoint;

    [Header("Shooting Settings")]

    [Tooltip("How many bullets the player can shoot per second.")]
    [SerializeField] private float fireRate = 5f;

    [Tooltip("Extra damage added to every bullet fired by the player.")]
    [SerializeField] private int bulletDamageBonus = 0;

    // Controls when the player can shoot again.
    private float nextFireTime;

    private void Update()
    {
        // Check if the player is pressing the fire button.
        HandleShootingInput();
    }

    /// Reads player shooting input and fires if the cooldown is ready.
    private void HandleShootingInput()
    {
        // Left mouse button held down.
        bool isShooting = Input.GetMouseButton(0);

        // Only shoot if the player is pressing fire and enough time has passed.
        if (isShooting && Time.time >= nextFireTime)
        {
            Shoot();

            // Calculate the next time the player is allowed to shoot.
            // Example: fireRate 5 = one shot every 0.2 seconds.
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    /// Spawns a bullet at the FirePoint and sends it forward.
    private void Shoot()
    {
        // Safety check: do not shoot if prefab or fire point is missing.
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("PlayerShooting is missing Bullet Prefab or FirePoint.");
            return;
        }

        // Create the bullet at the FirePoint position and rotation.
        GameObject newBullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

      
        Vector2 shootDirection = firePoint.up;

        // Give the bullet its movement direction.
        Bullet bulletScript = newBullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetDirection(shootDirection);
            bulletScript.AddDamageBonus(bulletDamageBonus);
        }
    }

    /// Increases the player's fire rate.
    /// Used by level-up upgrades.
    public void IncreaseFireRate(float amount)
    {
        fireRate += amount;

        Debug.Log("Fire rate increased to: " + fireRate);
    }

    /// Increases the damage bonus applied to newly fired bullets.
    /// Used by level-up upgrades.
    public void IncreaseBulletDamage(int amount)
    {
        bulletDamageBonus += amount;

        Debug.Log("Bullet damage bonus increased to: " + bulletDamageBonus);
    }

    /// Returns the player's current fire rate.
    /// Used by the UI to display player stats.
    public float GetFireRate()
    {
        return fireRate;
    }

    /// Returns the player's total bullet damage.
    /// Base bullet damage is 1, plus any bonus from upgrades.
    public int GetCurrentBulletDamage()
    {
        return 1 + bulletDamageBonus;
    }
}