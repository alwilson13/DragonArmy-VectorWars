using System;
using UnityEngine;

public class PlayerXP : MonoBehaviour
{
    public event Action<int, int> OnXPChanged;
    public event Action<int> OnLevelChanged;

    [Header("Level Settings")]
    [SerializeField] int currentLevel = 1;
    [SerializeField] int currentXP = 0;
    [SerializeField] int xpToNextLevel = 100;
    [SerializeField] float xpGrowthMultiplier = 1.25f;

    public int CurrentLevel => currentLevel;
    public int CurrentXP => currentXP;
    public int XPToNextLevel => xpToNextLevel;

    public void AddXP(int amount)
    {
        currentXP += amount;

        while (currentXP >= xpToNextLevel)
        {
            currentXP -= xpToNextLevel;
            LevelUp();
        }

        OnXPChanged?.Invoke(currentXP, xpToNextLevel);
    }

    void LevelUp()
    {
        currentLevel++;

        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * xpGrowthMultiplier);

        OnLevelChanged?.Invoke(currentLevel);
    }
}