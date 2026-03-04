using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public struct InventoryCard
{
    
    public Card_SO cardSO; //The Card_SO this card gets its stats from
    public Hack_SO[] hacks; //All hacks that are attached to this card
    public int length;

    public string cardID;
  

    public void Awake()
    {
        if (hacks != null)
        {
            CheckLength();
        }
    }

    public InventoryCard(Card_SO cardSO, Hack_SO[] hacks, int length)
    {
        this.cardSO = cardSO;
        this.hacks = hacks;
        this.length = length;
        this.cardID = Guid.NewGuid().ToString();
    }

    public InventoryCard(Card_SO cardSO)
    {
        this.cardSO = cardSO;
        this.hacks = new Hack_SO[2];
        this.length = 2; 
        this.cardID = Guid.NewGuid().ToString();
    }

    public void CheckLength()
    {
        if (hacks.Length > length)
        {
            while (hacks.Length > length)
            {
                hacks[hacks.Length - 1] = null;
            }  
        }
    }
}