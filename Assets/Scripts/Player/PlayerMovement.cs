using UnityEngine;

/// Handles top-down player movement for Apex Vector.
/// It reads WASD / Arrow Key input and moves the player with Rigidbody2D.
public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]

    [Tooltip("How fast the player moves around the arena.")]
    [SerializeField] private float moveSpeed = 6f;

    // Reference to the Rigidbody2D component on the Player.
    private Rigidbody2D rb;

    // Stores movement input from the keyboard.
    private Vector2 moveInput;

    private void Awake()
    {
        // Get the Rigidbody2D attached to this Player object.
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        // Read horizontal input: A/D or Left/Right arrows.
        float moveX = Input.GetAxisRaw("Horizontal");

        // Read vertical input: W/S or Up/Down arrows.
        float moveY = Input.GetAxisRaw("Vertical");

        // Store the input as a Vector2.
        moveInput = new Vector2(moveX, moveY);

        // Normalize movement so diagonal movement is not faster.
        moveInput = moveInput.normalized;
    }

    private void FixedUpdate()
    {
        // Move the player using Rigidbody2D physics.
        rb.linearVelocity = moveInput * moveSpeed;
    }

    private void OnDisable()
    {
        // Stop movement when movement script gets disabled.
        // This is useful when the player dies or the game is paused.
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    /// Increases the player's movement speed.
    /// Used by level-up upgrades.
    public void IncreaseMoveSpeed(float amount)
    {
        moveSpeed += amount;

        Debug.Log("Move speed increased to: " + moveSpeed);
    }

    /// Returns the current movement input direction.
    /// Used by the dodge ability to know where the player wants to dash.
    public Vector2 GetMoveInput()
    {
        return moveInput;
    }
}