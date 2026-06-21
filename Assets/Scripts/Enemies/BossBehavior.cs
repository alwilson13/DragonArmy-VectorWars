using System.Collections;
using UnityEngine;

/// Controls the boss behavior.
/// 
/// The boss alternates between moving toward the player,
/// shooting projectiles at the player, and charging toward the player.
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

    [Header("Shooting Settings")]

    [Tooltip("Projectile prefab fired by the boss.")]
    [SerializeField] private GameObject bossProjectilePrefab;

    [Tooltip("How many aimed shots the boss fires before charging.")]
    [SerializeField] private int shotsPerAttack = 3;

    [Tooltip("Delay between each aimed shot.")]
    [SerializeField] private float timeBetweenShots = 0.35f;

    [Tooltip("How long the boss waits after shooting.")]
    [SerializeField] private float afterShootDelay = 0.8f;

    [Header("Charge Settings")]

    [Tooltip("How long the boss pauses before charging.")]
    [SerializeField] private float chargeWindupTime = 0.7f;

    [Tooltip("How fast the boss charges.")]
    [SerializeField] private float chargeSpeed = 8f;

    [Tooltip("How long the charge lasts.")]
    [SerializeField] private float chargeDuration = 0.45f;

    [Tooltip("How long the boss waits after charging.")]
    [SerializeField] private float afterChargeDelay = 1f;

    [Header("Visual Feedback")]

    [Tooltip("Should the boss flash before charging?")]
    [SerializeField] private bool flashBeforeCharge = true;

    [Tooltip("Boss color during charge windup.")]
    [SerializeField] private Color chargeWarningColor = Color.white;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;

    private Color originalColor;
    private bool isAttacking;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }
    }

    private void Start()
    {
        FindPlayerIfNeeded();

        StartCoroutine(BossAttackCycle());
    }

    private void FixedUpdate()
    {
        // During attacks, the coroutine controls movement.
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
    /// The boss shoots, waits, charges, waits, then repeats.
    private IEnumerator BossAttackCycle()
    {
        // Small delay so the boss does not attack instantly on spawn.
        yield return new WaitForSeconds(1.5f);

        while (true)
        {
            yield return StartCoroutine(ShootAttack());

            yield return new WaitForSeconds(afterShootDelay);

            yield return StartCoroutine(ChargeAttack());

            yield return new WaitForSeconds(afterChargeDelay);
        }
    }


    /// Boss fires several projectiles directly at the player.
    private IEnumerator ShootAttack()
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

    /// Spawns one projectile aimed at the player's current position.
    private void FireProjectileAtPlayer()
    {
        if (bossProjectilePrefab == null || playerTarget == null)
        {
            return;
        }

        Vector2 shootDirection = playerTarget.position - transform.position;

        GameObject newProjectile = Instantiate(
            bossProjectilePrefab,
            transform.position,
            Quaternion.identity
        );

        BossProjectile projectile = newProjectile.GetComponent<BossProjectile>();

        if (projectile != null)
        {
            projectile.SetDirection(shootDirection);
        }

        Debug.Log("Boss fired aimed shot.");
    }

    /// Boss locks onto the player's position, then charges in that direction.
    private IEnumerator ChargeAttack()
    {
        isAttacking = true;
        rb.linearVelocity = Vector2.zero;

        if (playerTarget == null)
        {
            isAttacking = false;
            yield break;
        }

        // Lock direction before the charge starts.
        Vector2 chargeDirection = playerTarget.position - transform.position;
        chargeDirection = chargeDirection.normalized;

        // Visual warning before charge.
        if (flashBeforeCharge && spriteRenderer != null)
        {
            spriteRenderer.color = chargeWarningColor;
        }

        Debug.Log("Boss is preparing to charge.");

        yield return new WaitForSeconds(chargeWindupTime);

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }

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

    private void OnDisable()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.color = originalColor;
        }
    }
}