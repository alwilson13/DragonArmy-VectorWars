using UnityEngine;

/// Damages the player when this enemy touches them.
/// 
/// Some enemies explode on contact.
/// Other enemies, like PatternMover, keep moving after contact.
public class EnemyContactDamage : MonoBehaviour
{
    [Header("Damage Settings")]

    [Tooltip("How much damage this enemy deals to the player on contact.")]
    [SerializeField] private int contactDamage = 1;

    [Tooltip("Should this enemy explode after hitting the player?")]
    [SerializeField] private bool explodeOnPlayerHit = true;

    [Tooltip("How long before this enemy can damage the player again.")]
    [SerializeField] private float damageCooldown = 0.75f;

    private float nextDamageTime;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryDamagePlayer(collision.gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        TryDamagePlayer(collision.gameObject);
    }

    /// Damages the player if cooldown is ready.
    private void TryDamagePlayer(GameObject otherObject)
    {
        if (!otherObject.CompareTag("Player"))
        {
            return;
        }

        if (Time.time < nextDamageTime)
        {
            return;
        }

        PlayerHealth playerHealth = otherObject.GetComponent<PlayerHealth>();

        if (playerHealth != null)
        {
            playerHealth.TakeDamage(contactDamage);
        }

        nextDamageTime = Time.time + damageCooldown;

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