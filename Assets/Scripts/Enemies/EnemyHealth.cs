using UnityEngine;

/// Handles enemy health, damage, death, score reward, XP drops, and explosion effects.
/// 
/// Enemies give score and drop XP when destroyed by player attacks.
/// Enemies do not give score or XP when they explode after hitting the player.
public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]

    [Tooltip("The enemy's maximum health.")]
    [SerializeField] private int maxHealth = 3;

    [Header("Score Settings")]

    [Tooltip("How many points the player earns for destroying this enemy.")]
    [SerializeField] private int scoreValue = 100;

    [Header("XP Settings")]

    [Tooltip("XP orb prefab spawned when this enemy is destroyed by the player.")]
    [SerializeField] private GameObject xpOrbPrefab;

    [Tooltip("How much XP this enemy drops when destroyed by the player.")]
    [SerializeField] private int xpValue = 25;

    [Header("Death Effects")]

    [Tooltip("Explosion prefab spawned when this enemy dies.")]
    [SerializeField] private GameObject explosionPrefab;

    // The enemy's current health during gameplay.
    private int currentHealth;

    // Prevents the enemy from dying more than once.
    private bool isDead;

    private void Awake()
    {
        // Start the enemy with full health.
        currentHealth = maxHealth;
    }

    /// Damages the enemy by a specific amount.
    /// This usually happens when the enemy is hit by a bullet.
    public void TakeDamage(int damageAmount)
    {
        // Do not take damage if already dead.
        if (isDead)
        {
            return;
        }

        currentHealth -= damageAmount;

        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Health left: " + currentHealth);

        // If health is zero or below, kill the enemy and reward the player.
        if (currentHealth <= 0)
        {
            Die(true);
        }
    }

    /// Instantly kills the enemy without giving score or XP.
    /// This is used when the enemy hits the player and explodes.
    public void ExplodeAndDie()
    {
        if (isDead)
        {
            return;
        }

        // False means no score and no XP are awarded.
        Die(false);
    }

    /// Handles enemy death.
    /// Can optionally award score and drop XP depending on how the enemy died.
    private void Die(bool rewardPlayer)
    {
        isDead = true;

        Debug.Log(gameObject.name + " exploded.");

        // Reward the player only if the enemy was destroyed by player damage.
        if (rewardPlayer)
        {
            AwardScore();
            DropXP();
            TryDropWeaponPickup();
            TryDropUpgradeOrb();
        }

        SpawnExplosion();

        Destroy(gameObject);
    }

    /// Adds score to the ScoreManager.
    private void AwardScore()
    {
        ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>();

        if (scoreManager != null)
        {
            scoreManager.AddScore(scoreValue);
        }
    }

    /// Spawns an XP orb at the enemy's position.
    private void DropXP()
    {
        if (xpOrbPrefab == null)
        {
            return;
        }

        GameObject newXPOrb = Instantiate(
            xpOrbPrefab,
            transform.position,
            Quaternion.identity
        );

        XPOrb xpOrb = newXPOrb.GetComponent<XPOrb>();

        if (xpOrb != null)
        {
            xpOrb.SetXPValue(xpValue);
        }
    }

    /// Gives the PickupDropManager a chance to spawn a weapon pickup.
    /// This only happens when the enemy was killed by player damage.
    private void TryDropWeaponPickup()
    {
        PickupDropManager pickupDropManager = FindFirstObjectByType<PickupDropManager>();

        if (pickupDropManager != null)
        {
            pickupDropManager.TryDropWeaponPickup(transform.position);
        }
    }

    /// Gives the UpgradeDropManager a chance to spawn an automatic upgrade orb.
    /// This only happens when the enemy was killed by player damage.
    private void TryDropUpgradeOrb()
    {
        UpgradeDropManager upgradeDropManager = FindFirstObjectByType<UpgradeDropManager>();

        if (upgradeDropManager != null)
        {
            upgradeDropManager.TryDropUpgradeOrb(transform.position);
        }
    }

    /// Spawns the enemy explosion effect and plays explosion sound.
    private void SpawnExplosion()
    {
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayEnemyExplosionSFX();
        }

        if (CameraShake.Instance != null)
        {
            CameraShake.Instance.Shake(0.08f, 0.08f);
        }
    }
}