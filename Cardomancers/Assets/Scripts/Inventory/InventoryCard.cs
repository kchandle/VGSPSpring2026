using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct InventoryCard
{
    
    public Card_SO cardSO; //The Card_SO this card gets its stats from
    public List<Hack_SO> hacks; //All hacks that are attached to this card
    public int length;

    public void Awake()
    {
        if (hacks != null)
        {
            CheckLength();
        }
    }

    public InventoryCard(Card_SO cardSO, List<Hack_SO> hacks, int length)
    {
        this.cardSO = cardSO;
        this.hacks = hacks;
        this.length = length;
    }

    public void CheckLength()
    {
        if (hacks.Count > length)
        {
            while (hacks.Count > length)
            {
                hacks.RemoveAt(hacks.Count - 1);
            }  
        }
    }
}