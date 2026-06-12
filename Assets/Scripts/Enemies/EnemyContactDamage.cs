using UnityEngine;

/// Allows an enemy to damage the player when touching them.
/// 
/// By default, enemies explode after hitting the player.
public class EnemyContactDamage : MonoBehaviour
{
    [Header("Damage Settings")]

    [Tooltip("How much damage this enemy deals to the player on contact.")]
    [SerializeField] private int contactDamage = 1;

    [Tooltip("Should this enemy explode after hitting the player?")]
    [SerializeField] private bool explodeOnPlayerHit = true;

    // Prevents the enemy from hitting the player multiple times before being destroyed.
    private bool hasHitPlayer;

    /// Called by Unity when this enemy's collider touches another non-trigger collider.
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Stop if this enemy already hit the player.
        if (hasHitPlayer)
        {
            return;
        }

        // Check if the enemy touched the player.
        if (collision.gameObject.CompareTag("Player"))
        {
            hasHitPlayer = true;

            // Try to get the PlayerHealth script from the player.
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            // If the player has health, damage them.
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(contactDamage);
            }

            // Explode and destroy this enemy after contact.
            if (explodeOnPlayerHit)
            {
                EnemyHealth enemyHealth = GetComponent<EnemyHealth>();

                if (enemyHealth != null)
                {
                    enemyHealth.ExplodeAndDie();
                }
                else
                {
                    // Fallback in case EnemyHealth is missing.
                    Destroy(gameObject);
                }
            }
        }
    }
}