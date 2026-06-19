using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
 * CameraFollow.cs
 *
 * Purpose:
 * Keeps the camera centered on the player using
 * a configurable positional offset.
 *
 * Inspiration:
 * Common Unity follow-camera patterns used in
 * top-down and arcade-style games.
 *
 * Unity References:
 * GameObject.FindWithTag()
 * Transform.position
 * LateUpdate()
 *
 * Adaptations:
 * - Automatic player lookup.
 * - Configurable camera offset.
 * - Designed for Vector Wars top-down gameplay.
 */

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