using System.Collections.Generic;
using UnityEngine;

/// Moves an enemy around the arena using patrol points.
/// 
/// This enemy travels across the arena instead of chasing the player.
/// It auto-finds GameObjects tagged ArenaPatrolPoint, making it safe for spawned prefabs.
public class ArenaPatrolEnemy : MonoBehaviour
{
    public enum PatrolMode
    {
        Loop,
        PingPong,
        Random
    }

    [Header("Patrol Settings")]

    [Tooltip("The points this enemy will move between. If empty, it finds objects tagged ArenaPatrolPoint.")]
    [SerializeField] private Transform[] patrolPoints;

    [Tooltip("How the enemy moves through patrol points.")]
    [SerializeField] private PatrolMode patrolMode = PatrolMode.Loop;

    [Tooltip("How fast the enemy moves.")]
    [SerializeField] private float moveSpeed = 3f;

    [Tooltip("How close the enemy must get to a patrol point before choosing the next one.")]
    [SerializeField] private float pointReachDistance = 0.15f;

    [Tooltip("Should the enemy rotate toward its movement direction?")]
    [SerializeField] private bool rotateTowardMovement = true;

    [Header("Arena Boundaries")]

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

    private Rigidbody2D rb;

    private int currentPointIndex = 0;
    private int direction = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        FindPatrolPointsIfNeeded();

        ClampStartingPosition();

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogWarning(gameObject.name + " has no patrol points assigned or found.");
        }
    }

    private void FixedUpdate()
    {
        MoveAlongPatrolPath();
    }

    /// Finds patrol points in the scene if none were assigned in the Inspector.
    private void FindPatrolPointsIfNeeded()
    {
        if (patrolPoints != null && patrolPoints.Length > 0)
        {
            return;
        }

        GameObject[] foundPoints = GameObject.FindGameObjectsWithTag("ArenaPatrolPoint");

        List<Transform> pointTransforms = new List<Transform>();

        foreach (GameObject point in foundPoints)
        {
            pointTransforms.Add(point.transform);
        }

        // Sort points by name so the order is predictable:
        // PatrolPoint_0, PatrolPoint_1, PatrolPoint_2, etc.
        pointTransforms.Sort((a, b) => a.name.CompareTo(b.name));

        patrolPoints = pointTransforms.ToArray();
    }

    /// Keeps the enemy starting position inside arena bounds.
    private void ClampStartingPosition()
    {
        if (!useArenaLimits)
        {
            return;
        }

        Vector2 clampedPosition = transform.position;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        transform.position = clampedPosition;
    }

    /// Moves the enemy toward the current patrol point.
    private void MoveAlongPatrolPath()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Transform targetPoint = patrolPoints[currentPointIndex];

        if (targetPoint == null)
        {
            ChooseNextPoint();
            return;
        }

        Vector2 targetPosition = targetPoint.position;
        Vector2 currentPosition = rb.position;

        Vector2 moveDirection = targetPosition - currentPosition;
        float distanceToPoint = moveDirection.magnitude;

        if (distanceToPoint <= pointReachDistance)
        {
            ChooseNextPoint();
            return;
        }

        moveDirection = moveDirection.normalized;

        rb.linearVelocity = moveDirection * moveSpeed;

        if (rotateTowardMovement)
        {
            RotateTowardDirection(moveDirection);
        }

        KeepInsideArena();
    }

    /// Chooses the next patrol point based on the selected patrol mode.
    private void ChooseNextPoint()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            return;
        }

        switch (patrolMode)
        {
            case PatrolMode.Loop:
                currentPointIndex++;

                if (currentPointIndex >= patrolPoints.Length)
                {
                    currentPointIndex = 0;
                }
                break;

            case PatrolMode.PingPong:
                currentPointIndex += direction;

                if (currentPointIndex >= patrolPoints.Length)
                {
                    currentPointIndex = patrolPoints.Length - 2;
                    direction = -1;
                }
                else if (currentPointIndex < 0)
                {
                    currentPointIndex = 1;
                    direction = 1;
                }
                break;

            case PatrolMode.Random:
                currentPointIndex = Random.Range(0, patrolPoints.Length);
                break;
        }
    }

    /// Keeps the enemy inside the arena limits.
    private void KeepInsideArena()
    {
        if (!useArenaLimits)
        {
            return;
        }

        Vector2 clampedPosition = rb.position;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        rb.position = clampedPosition;
    }

    /// Rotates the enemy to face its movement direction.
    private void RotateTowardDirection(Vector2 directionToFace)
    {
        if (directionToFace == Vector2.zero)
        {
            return;
        }

        float angle = Mathf.Atan2(directionToFace.y, directionToFace.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

    private void OnDisable()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}