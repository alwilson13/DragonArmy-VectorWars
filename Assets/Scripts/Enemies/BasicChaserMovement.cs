using UnityEngine;

/// Handles movement for the Basic Chaser enemy.
/// 
/// The Basic Chaser is the simplest enemy type.
/// It constantly moves toward the player.
public class BasicChaserMovement : MonoBehaviour
{
    [Header("Movement Settings")]

    [Tooltip("How fast the enemy moves toward the player.")]
    [SerializeField] private float moveSpeed = 2.5f;

    [Header("Target Settings")]

    [Tooltip("The player that this enemy will chase.")]
    [SerializeField] private Transform playerTarget;

    // Reference to this enemy's Rigidbody2D.
    private Rigidbody2D rb;

    private void Awake()
    {
        // Get the Rigidbody2D attached to this enemy.
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // If no player target was assigned in the Inspector,
        // automatically find the GameObject tagged "Player".
        if (playerTarget == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                playerTarget = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("BasicChaserMovement could not find a GameObject tagged Player.");
            }
        }
    }

    private void FixedUpdate()
    {
        // Move the enemy toward the player during the physics update.
        MoveTowardPlayer();
    }

    /// Moves the enemy directly toward the player's current position.
    private void MoveTowardPlayer()
    {
        // If there is no player target, stop the enemy.
        if (playerTarget == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        // Calculate the direction from the enemy to the player.
        Vector2 directionToPlayer = playerTarget.position - transform.position;

        // Normalize the direction so the enemy moves at a consistent speed.
        directionToPlayer = directionToPlayer.normalized;

        // Move the enemy toward the player using Rigidbody2D velocity.
        rb.linearVelocity = directionToPlayer * moveSpeed;
    }
}