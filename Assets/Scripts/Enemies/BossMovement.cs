using UnityEngine;

/// Handles simple boss movement.
/// 
/// The boss slowly moves toward the player,
/// creating pressure without being too fast.

public class BossMovement : MonoBehaviour
{
    [Header("Movement Settings")]

    [Tooltip("How fast the boss moves toward the player.")]
    [SerializeField] private float moveSpeed = 1.2f;

    [Tooltip("How close the boss tries to get before slowing down.")]
    [SerializeField] private float stopDistance = 3f;

    [Header("Target Settings")]

    [Tooltip("The player target.")]
    [SerializeField] private Transform playerTarget;

    // Rigidbody2D reference.
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // Find player automatically if not assigned.
        if (playerTarget == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                playerTarget = playerObject.transform;
            }
        }
    }

    private void FixedUpdate()
    {
        MoveBoss();
    }

    /// Moves the boss toward the player until it reaches stop distance.
    private void MoveBoss()
    {
        if (playerTarget == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Vector2 directionToPlayer = playerTarget.position - transform.position;
        float distanceToPlayer = directionToPlayer.magnitude;

        if (distanceToPlayer <= stopDistance)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        directionToPlayer = directionToPlayer.normalized;

        rb.linearVelocity = directionToPlayer * moveSpeed;
    }

    private void OnDisable()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}