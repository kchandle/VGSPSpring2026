#nullable enable
using UnityEngine;

public class InventoryHack : PlayItem
{
    [SerializeField] private Hack_SO hack;
    private Card cardToAddHack;
    private Playspace playspace;

    private void Awake()
    {
        hack = GetComponentInParent<Hack_SO>();
    }

    public void AddHackToCard()
    {
        if(cardToAddHack.hacks.Count < cardToAddHack.maxHacks)
        {
            cardToAddHack.hacks.Add(hack);
        }
        else
        {
            Debug.LogWarning("This card has the maximum number of hacks");
        }
    }

    private Card? FindCard()
    {
        foreach (PlayItem item in playspace.playItems)
        {
            if (item.BoxCollider.IsTouching(this.BoxCollider) && item is Card)
            {
                return item as Card;
            }
        }
        return null;
    }

    private void Update()
    {
        cardToAddHack = FindCard();
    }
}
