using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Player2DController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] float moveSpeed = 7f;
    [SerializeField] float acceleration = 20f;
    [SerializeField] float deceleration = 25f;

    Rigidbody2D rb;
    Vector2 moveInput;
    Vector2 currentVelocity;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0f;
        rb.freezeRotation = true;
    }

    void Update()
    {
        moveInput = new Vector2(
            Input.GetAxisRaw("Horizontal"),
            Input.GetAxisRaw("Vertical")
        ).normalized;
    }

    void FixedUpdate()
    {
        Vector2 targetVelocity = moveInput * moveSpeed;

        float accelRate = moveInput.magnitude > 0 ? acceleration : deceleration;

        currentVelocity = Vector2.MoveTowards(
            currentVelocity,
            targetVelocity,
            accelRate * Time.fixedDeltaTime
        );

        rb.linearVelocity = currentVelocity;
    }
}