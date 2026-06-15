using UnityEngine;

public class XPPickup : MonoBehaviour
{
    public enum XPType
    {
        Small,
        Medium,
        Large
    }

    [Header("XP Settings")]
    [SerializeField] private XPType xpType;

    private int xpAmount;

    public void SetXPAmount(int amount)
    {
        xpAmount = amount;
    }

    private void Start()
    {
        switch (xpType)
        {
            case XPType.Small:
                xpAmount = 1;
                break;

            case XPType.Medium:
                xpAmount = 5;
                break;

            case XPType.Large:
                xpAmount = 25;
                break;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerLevel playerLevel = other.GetComponent<PlayerLevel>();

        if (playerLevel != null)
        {
            playerLevel.GainXP(xpAmount);
        }

        Destroy(gameObject);
    }
}