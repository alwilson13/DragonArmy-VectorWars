using System.Collections;
using UnityEngine;

/// Visual warning ring used before the boss charges.
/// 
/// The ring follows the boss, pulses, grows slightly, fades out,
/// then destroys itself.
public class BossChargeRing : MonoBehaviour
{
    [Header("Ring Shape")]

    [SerializeField] private float radius = 1.1f;
    [SerializeField] private int segments = 64;

    [Header("Animation Settings")]

    [SerializeField] private float pulseSpeed = 10f;
    [SerializeField] private float pulseAmount = 0.12f;
    [SerializeField] private float finalScaleMultiplier = 1.35f;
    [SerializeField] private float startWidth = 0.1f;
    [SerializeField] private float endWidth = 0.02f;

    private LineRenderer lineRenderer;
    private Transform followTarget;
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
            Debug.LogWarning("BossChargeRing is missing a LineRenderer.");
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

    /// Starts the warning ring animation.
    public void StartRing(float duration, Transform targetToFollow)
    {
        followTarget = targetToFollow;

        StartCoroutine(RingRoutine(duration));
    }

    /// Keeps the ring on the boss while it pulses and fades.
    private IEnumerator RingRoutine(float duration)
    {
        float timer = 0f;

        Color startColor = Color.yellow;

        if (lineRenderer != null)
        {
            startColor = lineRenderer.startColor;
        }

        while (timer < duration)
        {
            timer += Time.deltaTime;

            float progress = timer / duration;

            if (followTarget != null)
            {
                transform.position = followTarget.position;
            }

            float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;
            float grow = Mathf.Lerp(1f, finalScaleMultiplier, progress);

            transform.localScale = startScale * pulse * grow;

            float currentWidth = Mathf.Lerp(startWidth, endWidth, progress);

            if (lineRenderer != null)
            {
                lineRenderer.startWidth = currentWidth;
                lineRenderer.endWidth = currentWidth;

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