using UnityEngine;

/// Handles enemy health, damage, death, and explosion effects.
/// 
/// Any enemy with this script can take damage from bullets.
/// When health reaches zero, the enemy explodes and is destroyed.
public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]

    [Tooltip("The enemy's maximum health.")]
    [SerializeField] private int maxHealth = 3;

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
    public void TakeDamage(int damageAmount)
    {
        // Do not take damage if already dead.
        if (isDead)
        {
            return;
        }

        // Subtract damage from current health.
        currentHealth -= damageAmount;

        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Health left: " + currentHealth);

        // If health is zero or below, kill the enemy.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// Instantly kills the enemy.
    /// This is useful when the enemy hits the player and should explode.
    public void ExplodeAndDie()
    {
        if (isDead)
        {
            return;
        }

        Die();
    }

    /// Handles enemy death.
    /// Spawns an explosion effect, then destroys the enemy.
    private void Die()
    {
        isDead = true;

        Debug.Log(gameObject.name + " exploded.");

        // Spawn the explosion effect at the enemy's position.
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Destroy this enemy GameObject.
        Destroy(gameObject);
    }
}