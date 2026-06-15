using UnityEngine;

public class EnemyXPDrop : MonoBehaviour
{
    [Header("XP Drop")]
    [SerializeField] private GameObject xpPickupPrefab;
    [SerializeField] private int xpAmount = 1;

    public void DropXP()
    {
        if (xpPickupPrefab == null)
            return;

        GameObject xp = Instantiate(
            xpPickupPrefab,
            transform.position,
            Quaternion.identity);

        XPPickup pickup = xp.GetComponent<XPPickup>();

        if (pickup != null)
        {
            pickup.SetXPAmount(xpAmount);
        }
    }
}