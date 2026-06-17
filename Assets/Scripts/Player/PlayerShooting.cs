using UnityEngine;

/// Handles player shooting for Vector Wars.
/// 
/// The player can use different weapon types.
/// Standard fires one bullet.
/// Spread fires multiple bullets in a cone.
public class PlayerShooting : MonoBehaviour
{
    [Header("Shooting References")]

    [Tooltip("The bullet prefab that will be spawned when the player shoots.")]
    [SerializeField] private GameObject bulletPrefab;

    [Tooltip("The position where bullets will spawn.")]
    [SerializeField] private Transform firePoint;

    [Header("Shooting Settings")]

    [Tooltip("How many times the player can shoot per second.")]
    [SerializeField] private float fireRate = 5f;

    [Tooltip("Extra damage added to every bullet fired by the player.")]
    [SerializeField] private int bulletDamageBonus = 0;

    [Tooltip("The weapon the player is currently using.")]
    [SerializeField] private WeaponType currentWeapon = WeaponType.Standard;

    [Header("Spread Shot Settings")]

    [Tooltip("How many bullets the Spread weapon fires.")]
    [SerializeField] private int spreadBulletCount = 3;

    [Tooltip("The total angle of the spread cone.")]
    [SerializeField] private float spreadAngle = 30f;

    // Controls when the player can shoot again.
    private float nextFireTime;

    private void Update()
    {
        // Check if the player is pressing the fire button.
        HandleShootingInput();
    }

    /// Reads shooting input and fires when the cooldown is ready.
    private void HandleShootingInput()
    {
        // Left mouse button held down.
        bool isShooting = Input.GetMouseButton(0);

        if (isShooting && Time.time >= nextFireTime)
        {
            Shoot();

            // Example: fireRate 5 = one shot every 0.2 seconds.
            nextFireTime = Time.time + 1f / fireRate;
        }
    }

    /// Chooses which shooting behavior to use based on the current weapon.
    private void Shoot()
    {
        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("PlayerShooting is missing Bullet Prefab or FirePoint.");
            return;
        }

        switch (currentWeapon)
        {
            case WeaponType.Standard:
                ShootStandard();
                break;

            case WeaponType.Spread:
                ShootSpread();
                break;
        }
    }

    /// Fires one bullet straight forward.
    private void ShootStandard()
    {
        Vector2 shootDirection = GetForwardDirection();

        FireBullet(shootDirection);
    }

    /// Fires multiple bullets in a cone pattern.
    private void ShootSpread()
    {
        // If spread bullet count is 1 or less, behave like standard shot.
        if (spreadBulletCount <= 1)
        {
            ShootStandard();
            return;
        }

        // Get the middle forward direction.
        Vector2 forwardDirection = GetForwardDirection();

        // Start angle is negative half of the spread cone.
        float startAngle = -spreadAngle / 2f;

        // Space bullets evenly across the spread cone.
        float angleStep = spreadAngle / (spreadBulletCount - 1);

        for (int i = 0; i < spreadBulletCount; i++)
        {
            float currentAngle = startAngle + angleStep * i;

            Vector2 bulletDirection = RotateVector(forwardDirection, currentAngle);

            FireBullet(bulletDirection);
        }
    }

    /// Creates one bullet and sends it in the given direction.
    private void FireBullet(Vector2 shootDirection)
    {
        GameObject newBullet = Instantiate(
            bulletPrefab,
            firePoint.position,
            firePoint.rotation
        );

        Bullet bulletScript = newBullet.GetComponent<Bullet>();

        if (bulletScript != null)
        {
            bulletScript.SetDirection(shootDirection);
            bulletScript.AddDamageBonus(bulletDamageBonus);
        }
    }

    /// Returns the direction the FirePoint is facing.
    /// 
    /// Use firePoint.up if the green Y axis points out of the triangle tip.
    /// Use firePoint.right if the red X axis points out of the triangle tip.
    private Vector2 GetForwardDirection()
    {
        return firePoint.up;
    }

    /// Rotates a Vector2 by a number of degrees.
    /// Used to create spread shot angles.
    private Vector2 RotateVector(Vector2 vector, float degrees)
    {
        Quaternion rotation = Quaternion.Euler(0f, 0f, degrees);

        return rotation * vector;
    }

    /// Changes the player's current weapon.
    /// Called when the player collects a weapon pickup.
    public void SetWeapon(WeaponType newWeapon)
    {
        currentWeapon = newWeapon;

        Debug.Log("Weapon changed to: " + currentWeapon);
    }

    /// Returns the player's current weapon.
    /// Used by UI and weapon pickup logic.
    public WeaponType GetCurrentWeapon()
    {
        return currentWeapon;
    }

    /// Returns the current weapon name as text.
    /// Used by the UI.
    public string GetCurrentWeaponName()
    {
        return currentWeapon.ToString();
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