using UnityEngine;

/// Damages the player when this enemy touches them.
/// 
/// Some enemies explode on contact.
/// Other enemies, like PatternMover, ArenaPatroller, and Boss, can keep moving.
/// 
/// Important:
/// If the player is invincible, such as during a dodge,
/// the enemy should not damage the player and should not destroy itself.
public class EnemyContactDamage : MonoBehaviour
{
    [Header("Damage Settings")]

    [Tooltip("How much damage this enemy deals to the player on contact.")]
    [SerializeField] private int contactDamage = 1;

    [Tooltip("Should this enemy explode after successfully damaging the player?")]
    [SerializeField] private bool explodeOnPlayerHit = true;

    [Tooltip("How long before this enemy can damage the player again.")]
    [SerializeField] private float damageCooldown = 0.75f;

    // Prevents repeated damage too quickly.
    private float nextDamageTime;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamagePlayer(collision.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamagePlayer(collision.gameObject);
    }

    /// Attempts to damage the player.
    /// If the player is invincible, no damage happens and this enemy does not explode.
    private void TryDamagePlayer(GameObject otherObject)
    {
        // Only interact with the player.
        if (!otherObject.CompareTag("Player"))
        {
            return;
        }

        // Respect damage cooldown.
        if (Time.time < nextDamageTime)
        {
            return;
        }

        PlayerHealth playerHealth = otherObject.GetComponent<PlayerHealth>();

        if (playerHealth == null)
        {
            return;
        }

        // If the player is dodging/invincible, do nothing.
        // This prevents dodge from becoming a free enemy-destroying attack.
        if (playerHealth.IsInvincible())
        {
            return;
        }

        // Damage the player.
        playerHealth.TakeDamage(contactDamage);

        nextDamageTime = Time.time + damageCooldown;

        // Only explode if this enemy successfully damaged the player.
        if (explodeOnPlayerHit)
        {
            EnemyHealth enemyHealth = GetComponent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.ExplodeAndDie();
            }
        }
    }
}