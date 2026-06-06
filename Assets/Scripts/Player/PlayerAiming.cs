using UnityEngine;

/// Handles player aiming for Apex Vector.
/// 
/// This script rotates the player so the triangle points toward the mouse cursor.
public class PlayerAiming : MonoBehaviour
{
    [Header("Camera Reference")]

    [Tooltip("The camera used to convert the mouse position into world position.")]
    [SerializeField] private Camera mainCamera;

    [Header("Aiming Settings")]

    [Tooltip("Use this if your triangle sprite does not point to the right by default.")]
    [SerializeField] private float rotationOffset = 0f;

    // Stores the mouse position in world space.
    private Vector2 mouseWorldPosition;

    private void Awake()
    {
        // If no camera is assigned in the Inspector, use the main camera automatically.
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
    }

    private void Update()
    {
        // Read the current mouse position and rotate the player toward it.
        AimAtMouse();
    }

    /// Rotates the player so it faces the mouse cursor.
    private void AimAtMouse()
    {
        // Convert the mouse screen position into a world position.
        mouseWorldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);

        // Find the direction from the player to the mouse.
        Vector2 aimDirection = mouseWorldPosition - (Vector2)transform.position;

        // Convert that direction into an angle in degrees.
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        // Rotate the player around the Z axis.
        transform.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
    }
}