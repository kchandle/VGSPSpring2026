using System.Collections.Generic;
using UnityEngine;

public class InventoryCard
{
    
    public Card_SO cardSO; //The Card_SO this card gets its stats from
    public List<Hack_SO> hacks; //All hacks that are attached to this card

    public InventoryCard(Card card)
    {
        cardSO = card.CardSO;
        hacks = card.hacks;
    }

        public InventoryCard(Card_SO card)
    {
        cardSO = card;
    }
}