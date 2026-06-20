using UnityEngine;

/// Moves an enemy in a fixed pattern instead of chasing the player.
/// 
/// This enemy behaves like a moving hazard:
/// it follows a path and damages the player if the player gets in the way.
/// 
/// This version also keeps the enemy inside arena boundaries.
public class PatternMoverEnemy : MonoBehaviour
{
    public enum MovementPattern
    {
        Horizontal,
        Vertical,
        Circle,
        FigureEight
    }

    [Header("Movement Pattern")]

    [Tooltip("The movement pattern this enemy will follow.")]
    [SerializeField] private MovementPattern movementPattern = MovementPattern.Horizontal;

    [Tooltip("How fast the enemy moves through the pattern.")]
    [SerializeField] private float moveSpeed = 2f;

    [Tooltip("How far the enemy moves away from its starting position.")]
    [SerializeField] private float moveDistance = 3f;

    [Tooltip("How large the circle or figure-eight pattern is.")]
    [SerializeField] private float patternRadius = 2f;

    [Tooltip("Should the enemy rotate toward its movement direction?")]
    [SerializeField] private bool rotateTowardMovement = true;

    [Header("Arena Limits")]

    [Tooltip("Should this enemy stay inside the arena boundaries?")]
    [SerializeField] private bool useArenaLimits = true;

    [Tooltip("Minimum X position allowed.")]
    [SerializeField] private float minX = -8f;

    [Tooltip("Maximum X position allowed.")]
    [SerializeField] private float maxX = 8f;

    [Tooltip("Minimum Y position allowed.")]
    [SerializeField] private float minY = -4.5f;

    [Tooltip("Maximum Y position allowed.")]
    [SerializeField] private float maxY = 4.5f;

    // The position where this enemy spawned.
    private Vector3 startPosition;

    // Used to track movement direction for rotation.
    private Vector3 previousPosition;

    private void Start()
    {
        startPosition = transform.position;
        previousPosition = transform.position;

        ClampStartPositionInsideArena();
    }

    private void Update()
    {
        MoveInPattern();

        if (rotateTowardMovement)
        {
            RotateTowardMovementDirection();
        }
    }

    /// Keeps the enemy's starting position inside the arena.
    private void ClampStartPositionInsideArena()
    {
        if (!useArenaLimits)
        {
            return;
        }

        startPosition.x = Mathf.Clamp(startPosition.x, minX, maxX);
        startPosition.y = Mathf.Clamp(startPosition.y, minY, maxY);

        transform.position = startPosition;
    }

    /// Moves the enemy based on the selected pattern.
    private void MoveInPattern()
    {
        float time = Time.time * moveSpeed;

        Vector3 offset = Vector3.zero;

        switch (movementPattern)
        {
            case MovementPattern.Horizontal:
                offset = new Vector3(Mathf.Sin(time) * moveDistance, 0f, 0f);
                break;

            case MovementPattern.Vertical:
                offset = new Vector3(0f, Mathf.Sin(time) * moveDistance, 0f);
                break;

            case MovementPattern.Circle:
                offset = new Vector3(
                    Mathf.Cos(time) * patternRadius,
                    Mathf.Sin(time) * patternRadius,
                    0f
                );
                break;

            case MovementPattern.FigureEight:
                offset = new Vector3(
                    Mathf.Sin(time) * patternRadius,
                    Mathf.Sin(time * 2f) * patternRadius * 0.5f,
                    0f
                );
                break;
        }

        Vector3 targetPosition = startPosition + offset;

        // Keep final movement position inside the arena.
        if (useArenaLimits)
        {
            targetPosition.x = Mathf.Clamp(targetPosition.x, minX, maxX);
            targetPosition.y = Mathf.Clamp(targetPosition.y, minY, maxY);
        }

        transform.position = targetPosition;
    }

    /// Rotates the enemy to face the direction it is moving.
    private void RotateTowardMovementDirection()
    {
        Vector3 movementDirection = transform.position - previousPosition;

        if (movementDirection.sqrMagnitude > 0.001f)
        {
            float angle = Mathf.Atan2(movementDirection.y, movementDirection.x) * Mathf.Rad2Deg;

            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }

        previousPosition = transform.position;
    }
}