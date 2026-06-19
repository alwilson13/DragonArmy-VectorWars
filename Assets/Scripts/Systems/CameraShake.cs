using System.Collections;
using UnityEngine;

/// Adds a simple screen shake effect to the camera.
/// 
/// Other scripts can call CameraShake.Instance.Shake()
/// when explosions or player hits happen.
public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance { get; private set; }

    // Original local position of the camera.
    private Vector3 originalPosition;

    // Current shake coroutine.
    private Coroutine shakeCoroutine;

    private void Awake()
    {
        Instance = this;
        originalPosition = transform.localPosition;
    }

    /// Starts a camera shake.
    public void Shake(float duration, float strength)
    {
        if (shakeCoroutine != null)
        {
            StopCoroutine(shakeCoroutine);
        }

        shakeCoroutine = StartCoroutine(ShakeRoutine(duration, strength));
    }

    /// Performs the shake effect.
    private IEnumerator ShakeRoutine(float duration, float strength)
    {
        float timer = 0f;

        while (timer < duration)
        {
            float offsetX = Random.Range(-1f, 1f) * strength;
            float offsetY = Random.Range(-1f, 1f) * strength;

            transform.localPosition = originalPosition + new Vector3(offsetX, offsetY, 0f);

            timer += Time.deltaTime;

            yield return null;
        }

        transform.localPosition = originalPosition;
        shakeCoroutine = null;
    }
}