using UnityEngine;

/*
 * ScreenWrap.cs
 *
 * Purpose:
 * Wraps objects to the opposite side of the arena
 * when they leave the play area.
 *
 * Inspiration:
 * Asteroids-style arcade screen wrapping.
 *
 * Unity References:
 * Transform.position
 * OnDrawGizmosSelected()
 * Gizmos.DrawWireCube()
 * https://www.youtube.com/watch?v=msTxvFoygVU
 *
 * Adaptations:
 * - Configurable arena dimensions.
 * - Visual arena boundary gizmo.
 * - Integrated into Vector Wars arena gameplay.
 */

public class ScreenWrap : MonoBehaviour
{
    [Header("Arena Bounds")]
    [SerializeField] private float arenaWidth = 12f;
    [SerializeField] private float arenaHeight = 7f;

    private void LateUpdate()
    {
        Vector3 position = transform.position;

        // Left -> Right
        if (position.x < -arenaWidth)
        {
            position.x = arenaWidth;
        }
        // Right -> Left
        else if (position.x > arenaWidth)
        {
            position.x = -arenaWidth;
        }

        // Bottom -> Top
        if (position.y < -arenaHeight)
        {
            position.y = arenaHeight;
        }
        // Top -> Bottom
        else if (position.y > arenaHeight)
        {
            position.y = -arenaHeight;
        }

        transform.position = position;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;

        Gizmos.DrawWireCube(
            Vector3.zero,
            new Vector3(arenaWidth * 2, arenaHeight * 2, 0)
        );
    }
}