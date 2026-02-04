#nullable enable
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class InventoryHack : PlayItem
{
    // the hack that will be added
    [SerializeField] private Hack_SO hack;
    // the card that the hack will be added to
    private Card cardToAddHack;
    // List of all the playspaces in the scene
    private List<Playspace> playspaces;

    // populate playspace list
    private void Awake()
    {
        playspaces = FindObjectsByType<Playspace>(FindObjectsSortMode.None).ToList();
    }

    public void AddHackToCard()
    {
        if (!hack)
        {
            Debug.LogWarning("No Hack_SO attached");
            return;
        }
        if (!cardToAddHack)
        {
            Debug.LogWarning("No card detected");
            return;
        }
        if(cardToAddHack.hacks.Count < cardToAddHack.maxHacks)
        {
            cardToAddHack.hacks.Add(hack);
            Debug.Log(cardToAddHack.hacks);
        }
        else
        {
            Debug.LogWarning("This card has the maximum number of hacks");
        }
    }

    private Card? FindCard()
    {
        foreach (Playspace playspace in playspaces)
        {
            foreach (PlayItem item in playspace.playItems)
            {
                if (item.BoxCollider.IsTouching(this.BoxCollider) && item is Card)
                {
                    return item as Card;
                }
            }
        }

        return null;
    }

    private void Update()
    {
        cardToAddHack = FindCard();
    }
}
