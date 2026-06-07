using UnityEngine;

/// Allows an enemy to damage the player when touching them.
public class EnemyContactDamage : MonoBehaviour
{
    [Header("Damage Settings")]

    [Tooltip("How much damage this enemy deals to the player on contact.")]
    [SerializeField] private int contactDamage = 1;

    /// Called by Unity when this enemy's collider touches another non-trigger collider.
    private void OnCollisionStay2D(Collision2D collision)
    {
        // Check if the enemy is touching the player.
        if (collision.gameObject.CompareTag("Player"))
        {
            // Try to get the PlayerHealth script from the player.
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

            // If the player has health, damage them.
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(contactDamage);
            }
        }
    }
}