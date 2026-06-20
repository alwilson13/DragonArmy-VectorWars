using System.Collections;
using UnityEngine;

/// Handles the visual warning effect before an enemy spawns.
/// 
/// The indicator pulses for a short duration, then removes itself.
/// Enemy spawning is handled by EnemySpawner after the wait finishes.
public class SpawnIndicator : MonoBehaviour
{
    [Header("Visual Settings")]

    [Tooltip("How fast the indicator pulses.")]
    [SerializeField] private float pulseSpeed = 8f;

    [Tooltip("How much the indicator changes size while pulsing.")]
    [SerializeField] private float pulseAmount = 0.2f;

    [Tooltip("The final scale multiplier before disappearing.")]
    [SerializeField] private float finalScaleMultiplier = 1.4f;

    private SpriteRenderer spriteRenderer;
    private Vector3 startScale;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        startScale = transform.localScale;
    }

    /// Starts the indicator animation for the given duration.
    public void StartIndicator(float duration)
    {
        StartCoroutine(IndicatorRoutine(duration));
    }

    /// Pulses the indicator, fades it, then destroys the object.
    private IEnumerator IndicatorRoutine(float duration)
    {
        float timer = 0f;

        Color startColor = Color.red;

        if (spriteRenderer != null)
        {
            startColor = spriteRenderer.color;
        }

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float progress = timer / duration;

            // Pulse scale while also slowly growing the warning circle.
            float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            float grow = Mathf.Lerp(1f, finalScaleMultiplier, progress);

            transform.localScale = startScale * pulse * grow;

            // Fade out near the end.
            if (spriteRenderer != null)
            {
                Color newColor = startColor;
                newColor.a = Mathf.Lerp(startColor.a, 0f, progress);
                spriteRenderer.color = newColor;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}