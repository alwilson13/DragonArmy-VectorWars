using System.Collections;
using UnityEngine;

/// Handles movement for the Charger enemy.
/// 
/// The Charger tracks the player for a short time,
/// locks in a direction, charges forward quickly,
/// then pauses before charging again.
/// 
/// This makes the Charger feel different from the Basic Chaser.
public class ChargerMovement : MonoBehaviour
{
    [Header("Charge Settings")]

    [Tooltip("How fast the Charger moves while tracking the player.")]
    [SerializeField] private float trackingSpeed = 1.5f;

    [Tooltip("How fast the Charger moves during its charge.")]
    [SerializeField] private float chargeSpeed = 8f;

    [Tooltip("How long the Charger tracks the player before charging.")]
    [SerializeField] private float trackingTime = 1.2f;

    [Tooltip("How long the Charger continues charging forward.")]
    [SerializeField] private float chargeTime = 0.45f;

    [Tooltip("How long the Charger waits after charging before tracking again.")]
    [SerializeField] private float cooldownTime = 1.0f;

    [Header("Target Settings")]

    [Tooltip("The player that this enemy will attack.")]
    [SerializeField] private Transform playerTarget;

    // Reference to this enemy's Rigidbody2D.
    private Rigidbody2D rb;

    // Stores the direction used during the charge.
    private Vector2 chargeDirection;

    // Tracks whether the enemy behavior loop is active.
    private bool isBehaviorRunning;

    private void Awake()
    {
        // Get the Rigidbody2D attached to this enemy.
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        // If no player target was assigned, find the object tagged Player.
        if (playerTarget == null)
        {
            GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

            if (playerObject != null)
            {
                playerTarget = playerObject.transform;
            }
            else
            {
                Debug.LogWarning("ChargerMovement could not find a GameObject tagged Player.");
            }
        }

        // Start the Charger behavior loop.
        if (playerTarget != null)
        {
            StartCoroutine(ChargeBehaviorRoutine());
        }
    }

    /// Main behavior loop for the Charger.
    /// It repeats: track player, lock direction, charge, cooldown.
    private IEnumerator ChargeBehaviorRoutine()
    {
        isBehaviorRunning = true;

        while (isBehaviorRunning)
        {
            // Step 1: Track the player briefly.
            yield return StartCoroutine(TrackPlayerRoutine());

            // Step 2: Lock the charge direction toward the player's current position.
            LockChargeDirection();

            // Step 3: Charge forward in the locked direction.
            yield return StartCoroutine(ChargeRoutine());

            // Step 4: Stop and wait before charging again.
            yield return StartCoroutine(CooldownRoutine());
        }
    }

    /// Moves slowly toward the player before charging.
    /// This gives the player a small warning window.
    private IEnumerator TrackPlayerRoutine()
    {
        float timer = 0f;

        while (timer < trackingTime)
        {
            if (playerTarget == null)
            {
                rb.linearVelocity = Vector2.zero;
                yield break;
            }

            Vector2 directionToPlayer = playerTarget.position - transform.position;
            directionToPlayer = directionToPlayer.normalized;

            rb.linearVelocity = directionToPlayer * trackingSpeed;

            RotateTowardDirection(directionToPlayer);

            timer += Time.deltaTime;
            yield return null;
        }
    }

    /// Saves the direction the Charger will use for its charge.
    /// Once locked, the Charger will not keep correcting toward the player.
    private void LockChargeDirection()
    {
        if (playerTarget == null)
        {
            chargeDirection = Vector2.zero;
            return;
        }

        chargeDirection = playerTarget.position - transform.position;
        chargeDirection = chargeDirection.normalized;

        RotateTowardDirection(chargeDirection);
    }

    /// Moves very fast in the locked charge direction.
    private IEnumerator ChargeRoutine()
    {
        float timer = 0f;

        while (timer < chargeTime)
        {
            rb.linearVelocity = chargeDirection * chargeSpeed;

            timer += Time.deltaTime;
            yield return null;
        }
    }

    /// Stops the Charger briefly after a charge.
    private IEnumerator CooldownRoutine()
    {
        rb.linearVelocity = Vector2.zero;

        yield return new WaitForSeconds(cooldownTime);
    }

    /// Rotates the enemy so it faces the direction it is moving.
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
        // Stop movement if the script is disabled or the enemy is destroyed.
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        isBehaviorRunning = false;
    }
}