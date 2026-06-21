using System.Collections.Generic;
using UnityEngine;

/// Moves an enemy around the arena using patrol points.
/// 
/// This enemy auto-finds GameObjects tagged ArenaPatrolPoint,
/// then moves between them using Rigidbody2D.MovePosition.
/// This is more reliable than using linearVelocity for patrol movement.
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
    [SerializeField] private float pointReachDistance = 0.25f;

    [Tooltip("Should the enemy rotate toward its movement direction?")]
    [SerializeField] private bool rotateTowardMovement = true;

    [Header("Arena Boundaries")]

    [SerializeField] private bool useArenaLimits = true;
    [SerializeField] private float minX = -8f;
    [SerializeField] private float maxX = 8f;
    [SerializeField] private float minY = -4.5f;
    [SerializeField] private float maxY = 4.5f;

    private Rigidbody2D rb;
    private int currentPointIndex = 0;
    private int direction = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError(gameObject.name + " is missing Rigidbody2D.");
        }
    }

    private void Start()
    {
        FindPatrolPointsIfNeeded();

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            Debug.LogError(gameObject.name + " found 0 patrol points. Make sure patrol points are tagged ArenaPatrolPoint.");
            return;
        }

        Debug.Log(gameObject.name + " found " + patrolPoints.Length + " patrol points.");

        ClampStartingPosition();

        // If the enemy spawns very close to the first patrol point,
        // immediately choose the next one so it starts moving.
        if (Vector2.Distance(rb.position, patrolPoints[currentPointIndex].position) <= pointReachDistance)
        {
            ChooseNextPoint();
        }
    }

    private void FixedUpdate()
    {
        MoveAlongPatrolPath();
    }

    /// Finds patrol points in the scene if none were assigned.
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

        // Sort by name to create a predictable route:
        // PatrolPoint_0, PatrolPoint_1, PatrolPoint_2, etc.
        pointTransforms.Sort((a, b) => a.name.CompareTo(b.name));

        patrolPoints = pointTransforms.ToArray();
    }

    /// Moves the enemy toward the current patrol point.
    private void MoveAlongPatrolPath()
    {
        if (rb == null)
        {
            return;
        }

        if (patrolPoints == null || patrolPoints.Length == 0)
        {
            return;
        }

        Transform targetPoint = patrolPoints[currentPointIndex];

        if (targetPoint == null)
        {
            ChooseNextPoint();
            return;
        }

        Vector2 currentPosition = rb.position;
        Vector2 targetPosition = targetPoint.position;

        Vector2 directionToTarget = targetPosition - currentPosition;
        float distanceToTarget = directionToTarget.magnitude;

        if (distanceToTarget <= pointReachDistance)
        {
            ChooseNextPoint();
            return;
        }

        directionToTarget = directionToTarget.normalized;

        Vector2 nextPosition = currentPosition + directionToTarget * moveSpeed * Time.fixedDeltaTime;

        if (useArenaLimits)
        {
            nextPosition.x = Mathf.Clamp(nextPosition.x, minX, maxX);
            nextPosition.y = Mathf.Clamp(nextPosition.y, minY, maxY);
        }

        rb.MovePosition(nextPosition);

        if (rotateTowardMovement)
        {
            RotateTowardDirection(directionToTarget);
        }
    }

    /// Chooses the next patrol point based on patrol mode.
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

        Debug.Log(gameObject.name + " moving to patrol point: " + currentPointIndex);
    }

    /// Keeps starting position inside arena bounds.
    private void ClampStartingPosition()
    {
        if (!useArenaLimits || rb == null)
        {
            return;
        }

        Vector2 clampedPosition = rb.position;

        clampedPosition.x = Mathf.Clamp(clampedPosition.x, minX, maxX);
        clampedPosition.y = Mathf.Clamp(clampedPosition.y, minY, maxY);

        rb.position = clampedPosition;
    }

    /// Rotates enemy toward movement direction.
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