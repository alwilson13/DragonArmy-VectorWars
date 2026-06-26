using System.Collections;
using UnityEngine;

/// Handles the player's dodge ability.
/// 
/// The dodge gives the player a short burst of movement
/// and temporary invincibility.
/// 
/// Important:
/// Dodge input is ignored while the game is paused.
/// This prevents dodge input from being stored and executed after unpausing.
public class PlayerDodge : MonoBehaviour
{
    [Header("Dodge Settings")]

    [Tooltip("How fast the player moves during the dodge.")]
    [SerializeField] private float dodgeSpeed = 16f;

    [Tooltip("How long the dodge lasts.")]
    [SerializeField] private float dodgeDuration = 0.15f;

    [Tooltip("How long the player must wait before dodging again.")]
    [SerializeField] private float dodgeCooldown = 1f;

    [Tooltip("Should the player be invincible during the dodge?")]
    [SerializeField] private bool invincibleDuringDodge = true;

    [Header("References")]

    [Tooltip("Reference to the player's movement script.")]
    [SerializeField] private PlayerMovement playerMovement;

    [Tooltip("Reference to the player's health script.")]
    [SerializeField] private PlayerHealth playerHealth;

    // Rigidbody used to move the player during the dodge.
    private Rigidbody2D rb;

    // Tracks whether the player is currently dodging.
    private bool isDodging;

    // Tracks whether dodge is available.
    private bool canDodge = true;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (playerMovement == null)
        {
            playerMovement = GetComponent<PlayerMovement>();
        }

        if (playerHealth == null)
        {
            playerHealth = GetComponent<PlayerHealth>();
        }
    }

    private void Update()
    {
        // Do not allow dodge input while the game is paused.
        // This prevents Space from being stored and executed after unpausing.
        if (Time.timeScale == 0f)
        {
            return;
        }

        // Do not start another dodge while already dodging.
        if (isDodging)
        {
            return;
        }

        // Start dodge only when cooldown is ready.
        if (Input.GetKeyDown(KeyCode.Space) && canDodge)
        {
            StartCoroutine(DodgeRoutine());
        }
    }

    /// Performs the dodge movement and invincibility window.
    private IEnumerator DodgeRoutine()
    {
        // Extra safety check in case another script starts the coroutine while paused.
        if (Time.timeScale == 0f)
        {
            yield break;
        }

        isDodging = true;
        canDodge = false;

        Vector2 dodgeDirection = GetDodgeDirection();

        // Disable normal player movement during dodge so movement scripts do not fight each other.
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // Make player invincible during dodge if enabled.
        if (invincibleDuringDodge && playerHealth != null)
        {
            playerHealth.SetInvincible(true);
        }

        // Apply dodge velocity.
        if (rb != null)
        {
            rb.linearVelocity = dodgeDirection * dodgeSpeed;
        }

        // Wait for dodge duration.
        yield return new WaitForSeconds(dodgeDuration);

        // Stop dodge movement.
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        // Re-enable normal movement.
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        // Remove dodge invincibility.
        if (invincibleDuringDodge && playerHealth != null)
        {
            playerHealth.SetInvincible(false);
        }

        isDodging = false;

        // Wait for cooldown before allowing another dodge.
        yield return new WaitForSeconds(dodgeCooldown);

        canDodge = true;
    }

    /// Gets the direction the player should dodge.
    /// 
    /// If the player is moving, dodge in the movement direction.
    /// If the player is not moving, dodge forward based on player rotation.
    private Vector2 GetDodgeDirection()
    {
        Vector2 dodgeDirection = Vector2.zero;

        if (playerMovement != null)
        {
            dodgeDirection = playerMovement.GetMoveInput();
        }

        // If no movement input exists, dodge toward the direction the player is facing.
        if (dodgeDirection == Vector2.zero)
        {
            dodgeDirection = transform.up;
        }

        return dodgeDirection.normalized;
    }

    /// Reduces the dodge cooldown.
    /// Used by future upgrade systems if needed.
    public void ReduceCooldown(float amount)
    {
        dodgeCooldown -= amount;

        if (dodgeCooldown < 0.2f)
        {
            dodgeCooldown = 0.2f;
        }

        Debug.Log("Dodge cooldown reduced to: " + dodgeCooldown);
    }

    /// Returns whether the player can currently dodge.
    public bool CanDodge()
    {
        return canDodge && !isDodging;
    }

    private void OnDisable()
    {
        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        if (invincibleDuringDodge && playerHealth != null)
        {
            playerHealth.SetInvincible(false);
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        isDodging = false;
    }
}