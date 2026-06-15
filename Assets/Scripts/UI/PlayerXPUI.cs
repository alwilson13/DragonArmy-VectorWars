using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerXPUI : MonoBehaviour
{
    [SerializeField] private PlayerLevel playerLevel;
    [SerializeField] private Image xpBar;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text xpText;

    private void Start()
    {
        if (playerLevel == null)
        {
            playerLevel = FindFirstObjectByType<PlayerLevel>();
        }
    }

    private void Update()
    {
        if (playerLevel == null)
            return;

        xpBar.fillAmount = (float)playerLevel.GetCurrentXP() / playerLevel.GetXPToNextLevel();

        levelText.text = "Level " + playerLevel.GetCurrentLevel();
        xpText.text = playerLevel.GetCurrentXP() + " / " + playerLevel.GetXPToNextLevel() + " XP";
    }
}