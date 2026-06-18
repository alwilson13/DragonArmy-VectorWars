using UnityEngine;

/// Controls boss projectile movement and collision.
/// 
/// Boss projectiles move in a chosen direction,
/// damage the player on contact, and destroy themselves after a short lifetime.
public class BossProjectile : MonoBehaviour
{
    [Header("Projectile Settings")]

    [Tooltip("How fast the projectile moves.")]
    [SerializeField] private float speed = 5f;

    [Tooltip("How much damage this projectile deals to the player.")]
    [SerializeField] private int damage = 1;

    [Tooltip("How long the projectile exists before being destroyed.")]
    [SerializeField] private float lifetime = 4f;

    // Direction this projectile travels.
    private Vector2 moveDirection;

    // Rigidbody2D reference.
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Destroy projectile after its lifetime ends.
        Destroy(gameObject, lifetime);
    }

    /// Sets the projectile movement direction.
    /// Called by the boss when spawning projectiles.
    public void SetDirection(Vector2 direction)
    {
        moveDirection = direction.normalized;

        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = moveDirection * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Damage player if hit.
        if (collision.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }

            Destroy(gameObject);
        }

        // Destroy projectile if it hits arena wall.
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}