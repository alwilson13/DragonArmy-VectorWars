using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private Image healthBar;
    [SerializeField] private TMP_Text healthText;

    private void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = FindFirstObjectByType<PlayerHealth>();
        }
    }

    private void Update()
    {
        if (playerHealth == null)
            return;

        healthBar.fillAmount = (float)playerHealth.GetCurrentHealth() / playerHealth.GetMaxHealth();
        healthText.text = playerHealth.GetCurrentHealth() + " / " + playerHealth.GetMaxHealth() + " HP";
    }
}