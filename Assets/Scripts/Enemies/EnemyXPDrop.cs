using UnityEngine;

/*
 * EnemyXPDrop.cs
 *
 * Purpose:
 * Spawns an XP pickup when an enemy is defeated.
 *
 * References:
 * Unity Scripting API - Object.Instantiate()
 * https://docs.unity3d.com/ScriptReference/Object.Instantiate.html
 *
 * Adaptations:
 * - Customized for Vector Wars progression system.
 * - Uses project-specific XPPickup component.
 * - Supports configurable XP values per enemy.
 */

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