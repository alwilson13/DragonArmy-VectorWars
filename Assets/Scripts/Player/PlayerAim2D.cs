using UnityEngine;

public class PlayerAim2D : MonoBehaviour
{
    [Header("Aiming")]
    [SerializeField] Camera mainCamera;
    [SerializeField] Transform aimVisual;
    [SerializeField] float rotationOffset = -90f;

    Vector2 aimDirection;

    void Awake()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (aimVisual == null)
            aimVisual = transform;
    }

    void Update()
    {
        AimAtMouse();
    }

    void AimAtMouse()
    {
        Vector3 mouseScreenPos = Input.mousePosition;

        Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(new Vector3(
            mouseScreenPos.x,
            mouseScreenPos.y,
            Mathf.Abs(mainCamera.transform.position.z)
        ));

        mouseWorldPos.z = transform.position.z;

        aimDirection = mouseWorldPos - transform.position;

        if (aimDirection.sqrMagnitude <= 0.001f)
            return;

        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;

        aimVisual.rotation = Quaternion.Euler(0f, 0f, angle + rotationOffset);
    }

    public Vector2 GetAimDirection()
    {
        return aimDirection.normalized;
    }
}