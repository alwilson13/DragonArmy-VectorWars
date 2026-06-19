using UnityEngine;

/*
 * CameraFollow.cs
 *
 * Purpose:
 * Keeps the camera centered on the player using
 * a configurable positional offset.
 *
 * Inspiration:
 * Common Unity follow-camera patterns used in
 * top-down and arcade-style games.
 *
 * Unity References:
 * GameObject.FindWithTag()
 * Transform.position
 * LateUpdate()
 *
 * Adaptations:
 * - Automatic player lookup.
 * - Configurable camera offset.
 * - Designed for Vector Wars top-down gameplay.
 */

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0f, 0f, -10f);

    private void Start()
    {
        if (player == null)
        {
            GameObject playerObject = GameObject.FindWithTag("Player");

            if (playerObject != null)
            {
                player = playerObject.transform;
            }
        }
    }

    private void LateUpdate()
    {
        if (player == null)
            return;

        transform.position = player.position + offset;
    }
}