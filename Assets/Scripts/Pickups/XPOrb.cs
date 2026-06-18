using UnityEngine;

/// Handles XP orb pickup behavior.
/// 
/// When the player touches this orb, it gives XP to the player
/// and then destroys itself.
public class XPOrb : MonoBehaviour
{
    [Header("XP Settings")]

    [Tooltip("How much XP this orb gives to the player.")]
    [SerializeField] private int xpValue = 25;

    /// Called when another collider enters this trigger.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Only the player can collect XP orbs.
        if (collision.CompareTag("Player"))
        {
            PlayerExperience playerExperience = collision.GetComponent<PlayerExperience>();

            if (playerExperience != null)
            {
                playerExperience.AddXP(xpValue);
            }

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.PlayXPPickupSFX();
            }

            // Destroy the orb after collection.
            Destroy(gameObject);
        }
    }

    /// Lets enemy scripts set this orb's XP value when it is spawned.
    public void SetXPValue(int newXPValue)
    {
        xpValue = newXPValue;
    }
}