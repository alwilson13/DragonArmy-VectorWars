using System.Collections;
using UnityEngine;

/// Handles the player's dodge ability.
/// 
/// The player presses Space to quickly dash in their movement direction.
/// During the dodge, the player becomes briefly invincible.
/// The dodge has a cooldown to prevent constant spamming.
public class PlayerDodge : MonoBehaviour
{
    [Header("Dodge Settings")]

    [Tooltip("How fast the player moves during the dodge.")]
    [SerializeField] private float dodgeSpeed = 16f;

    [Tooltip("How long the dodge movement lasts.")]
    [SerializeField] private float dodgeDuration = 0.15f;

    [Tooltip("How long the player must wait before dodging again.")]
    [SerializeField] private float dodgeCooldown = 1f;

    [Tooltip("Should the player be invincible while dodging?")]
    [SerializeField] private bool invincibleDuringDodge = true;

    [Header("References")]

    [Tooltip("Reference to the player's movement script.")]
    [SerializeField] private PlayerMovement playerMovement;

    [Tooltip("Reference to the player's health script.")]
    [SerializeField] private PlayerHealth playerHealth;

    // Reference to the player's Rigidbody2D.
    private Rigidbody2D rb;

    // Tracks whether the player is currently dodging.
    private bool isDodging;

    // Tracks whether the dodge is on cooldown.
    private bool isOnCooldown;

    private void Awake()
    {
        // Get required components from the Player object.
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
        // Press Space to dodge.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TryDodge();
        }
    }

    /// Checks whether the player can dodge.
    /// If yes, starts the dodge routine.
    private void TryDodge()
    {
        // Do not dodge if already dodging or waiting on cooldown.
        if (isDodging || isOnCooldown)
        {
            return;
        }

        // Get the current movement direction from PlayerMovement.
        Vector2 dodgeDirection = Vector2.zero;

        if (playerMovement != null)
        {
            dodgeDirection = playerMovement.GetMoveInput();
        }

        // If the player is not pressing movement keys,
        // dodge in the direction the player is facing.
        if (dodgeDirection == Vector2.zero)
        {
            dodgeDirection = transform.up;
        }

        StartCoroutine(DodgeRoutine(dodgeDirection.normalized));
    }

    /// Performs the dodge movement, then starts the cooldown.
    private IEnumerator DodgeRoutine(Vector2 dodgeDirection)
    {
        isDodging = true;
        isOnCooldown = true;

        // Temporarily disable regular movement so it does not fight the dodge velocity.
        if (playerMovement != null)
        {
            playerMovement.enabled = false;
        }

        // Make the player invincible during the dodge.
        if (invincibleDuringDodge && playerHealth != null)
        {
            playerHealth.SetInvincible(true);
        }

        float timer = 0f;

        while (timer < dodgeDuration)
        {
            rb.linearVelocity = dodgeDirection * dodgeSpeed;

            timer += Time.deltaTime;
            yield return null;
        }

        // Stop dodge velocity.
        rb.linearVelocity = Vector2.zero;

        // Turn invincibility off after the dodge ends.
        if (invincibleDuringDodge && playerHealth != null)
        {
            playerHealth.SetInvincible(false);
        }

        // Re-enable regular movement.
        if (playerMovement != null)
        {
            playerMovement.enabled = true;
        }

        isDodging = false;

        // Wait for cooldown before allowing another dodge.
        yield return new WaitForSeconds(dodgeCooldown);

        isOnCooldown = false;
    }

    /// Reduces the dodge cooldown.
    /// Useful later for level-up upgrades.
    public void ReduceCooldown(float amount)
    {
        dodgeCooldown -= amount;

        // Prevent cooldown from becoming too low.
        dodgeCooldown = Mathf.Max(0.2f, dodgeCooldown);

        Debug.Log("Dodge cooldown reduced to: " + dodgeCooldown);
    }

    /// Returns whether dodge is currently available.
    /// Useful later for UI.
    public bool CanDodge()
    {
        return !isDodging && !isOnCooldown;
    }
}