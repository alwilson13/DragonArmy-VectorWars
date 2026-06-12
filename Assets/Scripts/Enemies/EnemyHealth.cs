using UnityEngine;

/// Handles enemy health, damage, death, score reward, and explosion effects.
/// 
/// Enemies give score when destroyed by player attacks.
/// Enemies do not give score when they explode after hitting the player.
public class EnemyHealth : MonoBehaviour
{
    [Header("Health Settings")]

    [Tooltip("The enemy's maximum health.")]
    [SerializeField] private int maxHealth = 3;

    [Header("Score Settings")]

    [Tooltip("How many points the player earns for destroying this enemy.")]
    [SerializeField] private int scoreValue = 100;

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

        // Subtract damage from current health.
        currentHealth -= damageAmount;

        Debug.Log(gameObject.name + " took " + damageAmount + " damage. Health left: " + currentHealth);

        // If health is zero or below, kill the enemy and award score.
        if (currentHealth <= 0)
        {
            Die(true);
        }
    }

    /// Instantly kills the enemy without giving score.
    /// This is used when the enemy hits the player and explodes.
    public void ExplodeAndDie()
    {
        if (isDead)
        {
            return;
        }

        // False means no score is awarded.
        Die(false);
    }

    /// Handles enemy death.
    /// Can optionally award score depending on how the enemy died.
    private void Die(bool awardScore)
    {
        isDead = true;

        Debug.Log(gameObject.name + " exploded.");

        // Award score only if the player destroyed the enemy.
        if (awardScore)
        {
            ScoreManager scoreManager = FindFirstObjectByType<ScoreManager>();

            if (scoreManager != null)
            {
                scoreManager.AddScore(scoreValue);
            }
        }

        // Spawn the explosion effect at the enemy's position.
        if (explosionPrefab != null)
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        }

        // Destroy this enemy GameObject.
        Destroy(gameObject);
    }
}