using System;
using UnityEngine;

public class TestQuestStep : QuestStep
{
   private int cardsPickedUp;
   private readonly int cardsToPickUp = 2;
   
   private void OnEnable()
   {
      cardPickup[] cardPickups = FindObjectsByType<cardPickup>(FindObjectsSortMode.None);

      foreach (cardPickup c in cardPickups)
      {
         c.GetCard.AddListener(IncrementCardsNumber);
         
      }
   }

   private void OnDisable()
   {
      cardPickup[] cardPickups = FindObjectsByType<cardPickup>(FindObjectsSortMode.None);
      foreach (cardPickup c in cardPickups)
      {
         c.GetCard.RemoveListener(IncrementCardsNumber);
      }
   }

   private void IncrementCardsNumber()
   {
      cardsPickedUp++;
      UpdateState();
      Debug.Log(cardsPickedUp);
      if (cardsPickedUp == cardsToPickUp)
      {
         FinishQuestStep();
      }
   }

   private void Start()
   {
      UpdateState();
   }

   private void UpdateState()
   {
      string state = cardsPickedUp.ToString();
      ChangeState(state);
   }

   protected override void SetQuestStepState(string str)
   {
      this.cardsPickedUp = System.Int32.Parse(str);
      UpdateState();
   }

   public override string GetQuestStepState()
   {
      return "Cards Picked Up: " + cardsPickedUp.ToString() + "/" + cardsToPickUp.ToString();
   }
}
