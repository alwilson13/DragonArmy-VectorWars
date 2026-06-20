using System.Collections;
using UnityEngine;

/// Creates and animates a circular ring warning indicator before an enemy spawns.
/// 
/// The ring pulses, grows, fades out, then destroys itself.
/// Enemy spawning is handled by EnemySpawner after the indicator duration finishes.
public class SpawnIndicatorRing : MonoBehaviour
{
    [Header("Ring Shape")]

    [Tooltip("Radius of the warning ring.")]
    [SerializeField] private float radius = 0.6f;

    [Tooltip("How smooth the ring should look.")]
    [SerializeField] private int segments = 64;

    [Header("Animation Settings")]

    [Tooltip("How fast the ring pulses.")]
    [SerializeField] private float pulseSpeed = 8f;

    [Tooltip("How much the ring changes size while pulsing.")]
    [SerializeField] private float pulseAmount = 0.15f;

    [Tooltip("How large the ring becomes before disappearing.")]
    [SerializeField] private float finalScaleMultiplier = 1.35f;

    [Tooltip("How thick the ring starts.")]
    [SerializeField] private float startWidth = 0.08f;

    [Tooltip("How thick the ring becomes near the end.")]
    [SerializeField] private float endWidth = 0.02f;

    private LineRenderer lineRenderer;
    private Vector3 startScale;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        startScale = transform.localScale;

        CreateRing();
    }

    /// Builds the circular ring using Line Renderer points.
    private void CreateRing()
    {
        if (lineRenderer == null)
        {
            Debug.LogWarning("SpawnIndicatorRing is missing a LineRenderer.");
            return;
        }

        lineRenderer.loop = true;
        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = segments;
        lineRenderer.startWidth = startWidth;
        lineRenderer.endWidth = startWidth;

        for (int i = 0; i < segments; i++)
        {
            float angle = ((float)i / segments) * Mathf.PI * 2f;

            float x = Mathf.Cos(angle) * radius;
            float y = Mathf.Sin(angle) * radius;

            lineRenderer.SetPosition(i, new Vector3(x, y, 0f));
        }
    }

    /// Starts the ring animation for the given duration.
    public void StartIndicator(float duration)
    {
        StartCoroutine(IndicatorRoutine(duration));
    }

    /// Pulses, grows, fades, and destroys the ring.
    private IEnumerator IndicatorRoutine(float duration)
    {
        float timer = 0f;

        Color startColor = Color.red;

        if (lineRenderer != null)
        {
            startColor = lineRenderer.startColor;
        }

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float progress = timer / duration;

            // Ring grows over time and also pulses.
            float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            float grow = Mathf.Lerp(1f, finalScaleMultiplier, progress);

            transform.localScale = startScale * pulse * grow;

            // Ring becomes thinner as spawn time gets closer.
            float currentWidth = Mathf.Lerp(startWidth, endWidth, progress);

            if (lineRenderer != null)
            {
                lineRenderer.startWidth = currentWidth;
                lineRenderer.endWidth = currentWidth;

                // Fade alpha out near the end.
                Color newColor = startColor;
                newColor.a = Mathf.Lerp(startColor.a, 0f, progress);

                lineRenderer.startColor = newColor;
                lineRenderer.endColor = newColor;
            }

            yield return null;
        }

        Destroy(gameObject);
    }
}