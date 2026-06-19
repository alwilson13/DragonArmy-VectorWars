using UnityEngine;

/// Makes a pickup gently pulse in size.
/// Useful for XP orbs and weapon pickups.
public class PulseEffect : MonoBehaviour
{
    [Tooltip("How fast the pulse animation plays.")]
    [SerializeField] private float pulseSpeed = 4f;

    [Tooltip("How strong the pulse size change is.")]
    [SerializeField] private float pulseAmount = 0.08f;

    // Original scale of the object.
    private Vector3 startScale;

    private void Start()
    {
        startScale = transform.localScale;
    }

    private void Update()
    {
        float pulse = 1f + Mathf.Sin(Time.time * pulseSpeed) * pulseAmount;

        transform.localScale = startScale * pulse;
    }
}