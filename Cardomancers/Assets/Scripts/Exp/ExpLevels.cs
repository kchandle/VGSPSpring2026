using System;

public static class ExpLevels
{
    private static readonly int maxLevels = 5;
    private static int currentLevel = 1;

    private static int expToNextLevel = 50;
    private static int currentExp = 0;

    private static int skillPoints;

    private static event Action levelUp;

    #region Properties
    public static int MaxLevels
    {
        get => maxLevels;
    }

    public static int CurrentLevel
    {
        get => currentLevel;
        set
        {
            currentLevel = value;
        }
    }

    public static int CurrentExp
    {
        get => currentExp;
        set
        {
            currentExp = value;
            if (currentExp >= expToNextLevel)
            {
                LevelUp();
            }
        }
    }
    #endregion
    
    #region Methods
    private static void LevelUp()
    {
        if (expToNextLevel > currentExp) return;
        if (currentLevel == maxLevels) return;
        
        currentLevel++;
        
        currentExp -= expToNextLevel;
        expToNextLevel = currentLevel * 50;

        skillPoints += currentLevel * 5;
        
        levelUp?.Invoke();
    }
    #endregion
}
