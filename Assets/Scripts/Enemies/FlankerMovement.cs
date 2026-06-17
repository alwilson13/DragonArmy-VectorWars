using UnityEngine;

/// Handles movement for the Flanker enemy.
/// 
/// The Flanker tries to move around the player instead of chasing directly.
/// It combines forward movement toward the player with sideways movement,
/// creating a curved/flanking path.
public class FlankerMovement : MonoBehaviour
{
    [Header("Movement Settings")]

    [Tooltip("How fast the Flanker moves.")]
    [SerializeField] private float moveSpeed = 3f;

    [Tooltip("How strongly the Flanker moves sideways around the player.")]
    [SerializeField] private float flankStrength = 1.5f;

    [Tooltip("How close the Flanker wants to get before circling more.")]
    [SerializeField] private float preferredDistance = 3f;

    [Tooltip("How often the Flanker changes its circling direction.")]
    [SerializeField] private float directionSwitchTime = 2f;

    [Header("Target Settings")]

    [Tooltip("The player that this enemy will flank.")]
    [SerializeField] private Transform playerTarget;

    // Reference to this enemy's Rigidbody2D.
    private Rigidbody2D rb;

    // Controls whether the Flanker circles clockwise or counter-clockwise.
    private int flankDirection = 1;

    // Timer used to switch flank direction.
    private float switchTimer;

    private void Awake()
    {
        // Get the Rigidbody2D attached to this enemy.
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Find the player automatically if no target was assigned.
        if (playerTarget == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                playerTarget = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("FlankerMovement could not find a GameObject tagged Player.");
            }
        }

        // Randomly choose initial flank direction.
        flankDirection = Random.value > 0.5f ? 1 : -1;
    }

    private void FixedUpdate()
    {
        MoveFlanker();
    }

    /// Moves the Flanker using a mix of toward-player and sideways movement.
    private void MoveFlanker()
    {
        if (playerTarget == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Direction from Flanker to player.
        Vector2 directionToPlayer = playerTarget.position - transform.position;

        float distanceToPlayer = directionToPlayer.magnitude;

        directionToPlayer = directionToPlayer.normalized;

        // Create a perpendicular direction for sideways/flanking movement.
        Vector2 sideDirection = new Vector2(-directionToPlayer.y, directionToPlayer.x) * flankDirection;

        // Switch side direction occasionally so movement feels less predictable.
        switchTimer += Time.fixedDeltaTime;

        if (switchTimer >= directionSwitchTime)
        {
            switchTimer = 0f;
            flankDirection *= -1;
        }

        Vector2 finalDirection;

        // If too far away, move more directly toward the player.
        if (distanceToPlayer > preferredDistance)
        {
            finalDirection = directionToPlayer + sideDirection * 0.5f;
        }
        else
        {
            // If close enough, circle/flank more aggressively.
            finalDirection = sideDirection * flankStrength + directionToPlayer * 0.3f;
        }

        finalDirection = finalDirection.normalized;

        rb.linearVelocity = finalDirection * moveSpeed;

        RotateTowardDirection(finalDirection);
    }

    /// Rotates the enemy to face the direction it is moving.
    /// Useful if the enemy sprite has a clear front direction.
    private void RotateTowardDirection(Vector2 direction)
    {
        if (direction == Vector2.zero)
        {
            return;
        }

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnDisable()
    {
        // Stop movement if this script is disabled.
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}