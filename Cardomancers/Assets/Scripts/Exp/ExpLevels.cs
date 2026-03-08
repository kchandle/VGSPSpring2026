using System;
using UnityEngine;

public static class ExpLevels
{
    // the highest level the player can reach
    private static readonly int maxLevels = 5;
    // the current level the player has
    private static int currentLevel = 1;

    //the amount of exp needed to level up
    private static int expToNextLevel = 50;
    // the current amount of exp the player has
    private static int currentExp = 0;

    // skill points, currently not used
    private static int skillPoints;

    // event that is raised when the player levels up
    private static event Action levelUp;

    #region Properties
    public static int MaxLevels
    {
        get => maxLevels;
    }

    public static int CurrentLevel
    {
        get => currentLevel;
    }

    // remove after tests are done
    public static int ExpToNextLevel
    {
        get => expToNextLevel;
    }

    public static int SkillPoints
    {
        get => skillPoints;
    }
    
    public static int CurrentExp
    {
        get => currentExp;
        set
        {
            currentExp = value;
            // calls the level up method when exp is high enough
            if (currentExp >= expToNextLevel)
            {
                LevelUp();
            }
            Debug.Log("New Current EXP = " + currentExp);
            Debug.Log("New Current LVL = " + currentLevel);
        }
    }
    #endregion
    
    #region Methods
    private static void LevelUp()
    {
        // if exp is not high enough, leave
        if (expToNextLevel > currentExp) return;
        // if at max level, don't level up
        if (currentLevel == maxLevels) return;
        
        // increases level by one
        currentLevel++;
        
        // reduce current exp by former exp to level
        currentExp -= expToNextLevel;
        // increase exp to next level based on the new level
        expToNextLevel = currentLevel * 50;

        // increase skill points based on next level
        skillPoints = currentLevel * 5;
        
        // call level up event
        levelUp?.Invoke();

        // If exp is still enough to level up, do it again
        if (currentExp >= expToNextLevel) LevelUp();
    }

    // updates fields based on save data
    public static void UpdateExpData(int newCurrentLevel, int newExpToNextLevel, int newCurrentExp, int newSkillPoints)
    {
        currentLevel = newCurrentLevel;
        expToNextLevel = newExpToNextLevel;
        currentExp = newCurrentExp;
        skillPoints = newSkillPoints;
    }
    #endregion
}
