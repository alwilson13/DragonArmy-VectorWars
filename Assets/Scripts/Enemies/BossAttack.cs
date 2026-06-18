using System.Collections;
using UnityEngine;

/// Handles boss attacks.
/// 
/// The boss fires projectiles in a circular ring pattern.
/// This creates a simple bullet-hell style attack for the final wave.
public class BossAttack : MonoBehaviour
{
    [Header("Projectile Settings")]

    [Tooltip("Projectile prefab fired by the boss.")]
    [SerializeField] private GameObject bossProjectilePrefab;

    [Tooltip("How many projectiles are fired in one ring attack.")]
    [SerializeField] private int projectilesPerRing = 12;

    [Tooltip("How often the boss attacks.")]
    [SerializeField] private float attackCooldown = 2.5f;

    [Tooltip("Small delay before the first attack.")]
    [SerializeField] private float firstAttackDelay = 1.5f;

    private void Start()
    {
        StartCoroutine(AttackRoutine());
    }

    /// Repeats boss attacks until the boss is destroyed.
    private IEnumerator AttackRoutine()
    {
        yield return new WaitForSeconds(firstAttackDelay);

        while (true)
        {
            FireRingAttack();

            yield return new WaitForSeconds(attackCooldown);
        }
    }

    /// Fires projectiles outward in a full circle.
    private void FireRingAttack()
    {
        if (bossProjectilePrefab == null)
        {
            Debug.LogWarning("BossAttack is missing boss projectile prefab.");
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

        Debug.Log("Boss fired ring attack.");
    }
}