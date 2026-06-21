using System.Collections;
using UnityEngine;

/// Controls the boss behavior.
/// 
/// The boss alternates between:
/// 1. Aimed shots at the player
/// 2. Ring bullet attack
/// 3. Charge attack with warning ring
/// 
/// Contact damage is handled by EnemyContactDamage.
public class BossBehavior : MonoBehaviour
{
    [Header("Target")]

    [Tooltip("The player target. If empty, the boss finds the Player tag automatically.")]
    [SerializeField] private Transform playerTarget;

    [Header("Movement Settings")]

    [Tooltip("How fast the boss moves normally.")]
    [SerializeField] private float moveSpeed = 1.2f;

    [Tooltip("How close the boss tries to get before slowing down.")]
    [SerializeField] private float stopDistance = 3f;

    [Header("Projectile Settings")]

    [Tooltip("Projectile prefab fired by the boss.")]
    [SerializeField] private GameObject bossProjectilePrefab;

    [Header("Aimed Shooting Settings")]

    [Tooltip("How many aimed shots the boss fires.")]
    [SerializeField] private int shotsPerAttack = 3;

    [Tooltip("Delay between each aimed shot.")]
    [SerializeField] private float timeBetweenShots = 0.35f;

    [Tooltip("How long the boss waits after aimed shooting.")]
    [SerializeField] private float afterAimedShotDelay = 0.7f;

    [Header("Ring Attack Settings")]

    [Tooltip("How many projectiles are fired in the ring attack.")]
    [SerializeField] private int projectilesPerRing = 12;

    [Tooltip("How long the boss waits after the ring attack.")]
    [SerializeField] private float afterRingAttackDelay = 0.9f;

    [Header("Charge Settings")]

    [Tooltip("Prefab for the yellow warning ring before charging.")]
    [SerializeField] private GameObject chargeRingPrefab;

    [Tooltip("How long the warning ring appears before the charge.")]
    [SerializeField] private float chargeWindupTime = 0.8f;

    [Tooltip("How fast the boss charges.")]
    [SerializeField] private float chargeSpeed = 8f;

    [Tooltip("How long the charge lasts.")]
    [SerializeField] private float chargeDuration = 0.45f;

    [Tooltip("How long the boss waits after charging.")]
    [SerializeField] private float afterChargeDelay = 1f;

    private Rigidbody2D rb;

    private bool isAttacking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        FindPlayerIfNeeded();

        StartCoroutine(BossAttackCycle());
    }

    private void FixedUpdate()
    {
        // During attack animations, the coroutine controls movement.
        if (isAttacking)
        {
            return;
        }

        MoveTowardPlayer();
    }

    /// Finds the player automatically if no target was assigned.
    private void FindPlayerIfNeeded()
    {
        if (playerTarget != null)
        {
            return;
        }

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if (playerObject != null)
        {
            playerTarget = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("BossBehavior could not find object tagged Player.");
        }
    }

    /// Normal boss movement between attacks.
    private void MoveTowardPlayer()
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

    /// Main boss attack cycle.
    private IEnumerator BossAttackCycle()
    {
        yield return new WaitForSeconds(1.5f);

        while (true)
        {
            yield return StartCoroutine(AimedShotAttack());

            yield return new WaitForSeconds(afterAimedShotDelay);

            RingAttack();

            yield return new WaitForSeconds(afterRingAttackDelay);

            yield return StartCoroutine(ChargeAttack());

            yield return new WaitForSeconds(afterChargeDelay);
        }
    }

    /// Boss fires several projectiles directly at the player.
    private IEnumerator AimedShotAttack()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        for (int i = 0; i < shotsPerAttack; i++)
        {
            FireProjectileAtPlayer();

            yield return new WaitForSeconds(timeBetweenShots);
        }

        isAttacking = false;
    }

    /// Fires one projectile toward the player's current position.
    private void FireProjectileAtPlayer()
    {
        if (bossProjectilePrefab == null || playerTarget == null)
        {
            return;
        }

        Vector2 shootDirection = playerTarget.position - transform.position;

        FireProjectile(shootDirection);

        Debug.Log("Boss fired aimed shot.");
    }

    /// Fires projectiles in a full circle around the boss.
    private void RingAttack()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        if (bossProjectilePrefab == null)
        {
            Debug.LogWarning("BossBehavior is missing Boss Projectile Prefab.");
            isAttacking = false;
            return;
        }

        float angleStep = 360f / projectilesPerRing;

        for (int i = 0; i < projectilesPerRing; i++)
        {
            float angle = i * angleStep;

            Vector2 direction = new Vector2(
                Mathf.Cos(angle * Mathf.Deg2Rad),
                Mathf.Sin(angle * Mathf.Deg2Rad)
            );

            FireProjectile(direction);
        }

        Debug.Log("Boss fired ring attack.");

        isAttacking = false;
    }

    /// Creates one boss projectile in the given direction.
    private void FireProjectile(Vector2 direction)
    {
        if (bossProjectilePrefab == null)
        {
            return;
        }

        GameObject newProjectile = Instantiate(
            bossProjectilePrefab,
            transform.position,
            Quaternion.identity
        );

        BossProjectile projectile = newProjectile.GetComponent<BossProjectile>();

        if (projectile != null)
        {
            projectile.SetDirection(direction);
        }
    }

    /// Boss shows a yellow warning ring, then charges toward the player's position.
    private IEnumerator ChargeAttack()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        if (playerTarget == null)
        {
            isAttacking = false;
            yield break;
        }

        Vector2 chargeDirection = playerTarget.position - transform.position;
        chargeDirection = chargeDirection.normalized;

        SpawnChargeWarningRing();

        Debug.Log("Boss is preparing to charge.");

        yield return new WaitForSeconds(chargeWindupTime);

        float chargeTimer = 0f;

        Debug.Log("Boss charged.");

        while (chargeTimer < chargeDuration)
        {
            rb.linearVelocity = chargeDirection * chargeSpeed;

            chargeTimer += Time.deltaTime;

            yield return null;
        }

        rb.linearVelocity = Vector2.zero;

        isAttacking = false;
    }

    /// Spawns the yellow charge warning ring around the boss.
    private void SpawnChargeWarningRing()
    {
        if (chargeRingPrefab == null)
        {
            return;
        }

        GameObject ringObject = Instantiate(
            chargeRingPrefab,
            transform.position,
            Quaternion.identity
        );

        BossChargeRing ring = ringObject.GetComponent<BossChargeRing>();

        if (ring != null)
        {
            ring.StartRing(chargeWindupTime, transform);
        }
        else
        {
            Destroy(ringObject, chargeWindupTime);
        }
    }

    private void OnDisable()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }
}