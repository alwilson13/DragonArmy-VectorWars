using UnityEngine;

/// Destroys this GameObject after a set amount of time.
public class DestroyAfterTime : MonoBehaviour
{
    [Tooltip("How long this object stays alive before being destroyed.")]
    [SerializeField] private float lifetime = 1f;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}