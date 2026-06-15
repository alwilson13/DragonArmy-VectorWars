using UnityEngine;

/// Handles enemy health, damage, and death.
/// 
/// Any enemy with this script can take damage from bullets.
/// When health reaches zero, the enemy is destroyed.
public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]

    [Tooltip("The enemy's maximum health.")]
    [SerializeField] private int maxHealth = 3;

    // The enemy's current health during gameplay.
    private int currentHealth;

    private void Awake()
    {
        // Start the enemy with full health.
        currentHealth = maxHealth;
    }

    /// Damages the enemy by a specific amount.
    public void TakeDamage(int damageAmount)
    {
        // Subtract damage from current health.
        currentHealth -= damageAmount;

        // Optional debug message so we can confirm damage is working.
        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Health left: " + currentHealth);

        // If health is zero or below, kill the enemy.
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    /// Handles enemy death.
    /// For now, the enemy is simply destroyed.
    private void Die()
    {
        Debug.Log(gameObject.name + " died.");

        EnemyXPDrop xpDrop = GetComponent<EnemyXPDrop>();

        if (xpDrop != null)
        {
            xpDrop.DropXP();
        }

        // Destroy this enemy GameObject.
        Destroy(gameObject);
    }
}