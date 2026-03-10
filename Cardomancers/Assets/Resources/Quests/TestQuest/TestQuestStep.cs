using UnityEngine;

public class TestQuestStep : QuestStep
{
   private void OnEnable()
   {
      cardPickup[] cardPickups = FindObjectsByType<cardPickup>(FindObjectsSortMode.None);

      foreach (cardPickup c in cardPickups)
      {
         c.GetCard.AddListener(FinishQuestStep);
      }
   }

   private void OnDisable()
   {
      cardPickup[] cardPickups = FindObjectsByType<cardPickup>(FindObjectsSortMode.None);
      foreach (cardPickup c in cardPickups)
      {
         c.GetCard.RemoveListener(FinishQuestStep);
      }
   }
}
