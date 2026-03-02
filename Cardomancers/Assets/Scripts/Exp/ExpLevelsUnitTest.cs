using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

public class ExpLevelsUnitTest
{
    [Test]
    public void MaxLevelTest()
    {
        Assert.IsTrue(ExpLevels.MaxLevels == 5);
    }

    [Test]
    [TestCase(25)]
    [TestCase(50)]
    [TestCase(150)]
    [TestCase(350)]
    [TestCase(750)]
    public void AddExpTest(int ExpToAdd)
    {
        ExpLevels.UpdateExpData(0,0,0,0);
       ExpLevels.CurrentExp += ExpToAdd;
       int expectedLevel = 1;
       switch (ExpToAdd)
       {
           case 25:
               expectedLevel = 1;
               break;
           case 50:
               expectedLevel = 2;
               break;
           case 150:
               expectedLevel = 3;
               break;
           case 350:
               expectedLevel = 4;
               break;
           case 750:
               expectedLevel = 5;
               break;
               
       }
       Assert.IsTrue(ExpLevels.CurrentLevel == expectedLevel);
       Assert.IsTrue(ExpLevels.ExpToNextLevel == expectedLevel * 50);
       Assert.IsTrue(ExpLevels.SkillPoints == expectedLevel * 5);
    }
}
