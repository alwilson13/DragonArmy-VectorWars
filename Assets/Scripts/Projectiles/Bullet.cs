using UnityEngine;

/// Controls the behavior of a bullet after it is fired.
/// 
/// The bullet moves in one direction, then destroys itself after a set lifetime.
/// Later, this script will also handle enemy damage when bullets hit enemies.
public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]

    [Tooltip("How fast the bullet moves.")]
    [SerializeField] private float bulletSpeed = 12f;

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
        // Destroy the bullet after its lifetime ends.
        // This prevents bullets from staying in the scene forever.
        Destroy(gameObject, lifetime);
    }

    /// Sets the bullet's travel direction.
    /// This is called by the PlayerShooting script when the bullet is created.
    public void SetDirection(Vector2 direction)
    {
        // Normalize makes sure the direction length is always 1.
        // This keeps bullet speed consistent.
        moveDirection = direction.normalized;

        // Rotate the bullet so it visually faces the direction it travels.
        float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void FixedUpdate()
    {
        // Move the bullet using Rigidbody2D physics.
        rb.linearVelocity = moveDirection * bulletSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // For now, destroy the bullet if it touches a wall.
        // Enemy damage will be added later
        if (collision.CompareTag("Wall"))
        {
            Destroy(gameObject);
        }
    }
}