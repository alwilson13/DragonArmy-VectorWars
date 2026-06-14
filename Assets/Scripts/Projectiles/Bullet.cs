using UnityEngine;

/// Controls bullet movement and collision.
/// 
/// The bullet moves in one direction, damages enemies on contact,
/// and destroys itself after hitting something or after its lifetime ends.
public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]

    [Tooltip("How fast the bullet moves.")]
    [SerializeField] private float bulletSpeed = 12f;

    [Tooltip("How much damage this bullet deals to enemies.")]
    [SerializeField] private int damage = 1;

    [Tooltip("How long the bullet stays alive before being destroyed.")]
    [SerializeField] private float lifetime = 2f;

    // The direction the bullet will travel.
    private Vector2 moveDirection;

    // Reference to the bullet's Rigidbody2D.
    private Rigidbody2D rb;

    private void Awake()
    {
        // Get the Rigidbody2D attached to this bullet.
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Destroy the bullet after a short time so bullets do not stay forever.
        Destroy(gameObject, lifetime);
    }

    /// Sets the bullet's travel direction.
    /// This is called from PlayerShooting when the bullet is spawned.
    public void SetDirection(Vector2 direction)
    {
        // Normalize direction so speed stays consistent.
        moveDirection = direction.normalized;

        // Rotate the bullet to face the direction it is moving.
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void FixedUpdate()
    {
        // Move bullet using Rigidbody2D physics.
        rb.linearVelocity = moveDirection * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // If the bullet hits an enemy, damage it and destroy the bullet.
        if (collision.CompareTag("Enemy"))
        {
            // Try to get the EnemyHealth script from the object we hit.
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();

            // If the enemy has the EnemyHealth script, apply damage.
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            // Destroy the bullet after hitting an enemy.
            Destroy(gameObject);
        }

        // If the bullet hits a wall, destroy the bullet.
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }

    /// Adds bonus damage to this bullet.
    /// Called by PlayerShooting when the bullet is created.
    public void AddDamageBonus(int bonusDamage)
    {
        damage += bonusDamage;
    }
}