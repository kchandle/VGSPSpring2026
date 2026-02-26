// #nullable enable
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

public class InventoryHack : PlayItem
{
    // the hack that will be added
    [SerializeField] private Hack_SO hackSO;
    // the card that the hack will be added to

    public Hack_SO HackSO
    {
        get {return hackSO;}
        set
        {
            hackSO = value;
            HackSprite = value.image;
        }
    }



    public Image hackImage;
    private Sprite hackSprite; 
    public Sprite HackSprite
    {
        get{return hackSprite;}
        set // when CardSprite is changed, also change it in the UI Image
        {
            hackSprite = value;
            hackImage.sprite = value;
        }
    }

    // List of all the playspaces in the scene
    private List<Playspace> playspaces;

    // populate playspace list
    private void Awake()
    {
        playspaces = FindObjectsByType<Playspace>(FindObjectsSortMode.None).ToList();
    }

    // public bool AddHackToCard(Card cardToAddHack)
    // {
    //     if (!hackSO)
    //     {
    //         Debug.LogWarning("No Hack_SO attached");
    //         return false;
    //     }
    //     else if (!cardToAddHack)
    //     {
    //         Debug.LogWarning("No card detected");
    //         return false;
    //     }
    //     else if(cardToAddHack.hacks.Count < cardToAddHack.maxHacks)
    //     {
    //         cardToAddHack.hacks.Add(hackSO);
    //         cardToAddHack.inventoryCard.hacks.Add(hackSO);
    //         Debug.Log(cardToAddHack.hacks);
    //         return true;
    //     }
    //     else
    //     {
    //         Debug.LogWarning("This card has the maximum number of hacks");
    //         return false;
    //     }
    // }

    // // private Card? FindCard()
    // {
    //     foreach (Playspace playspace in playspaces)
    //     {
    //         foreach (PlayItem item in playspace.playItems)
    //         {
    //             if (item.BoxCollider.IsTouching(this.BoxCollider) && item is Card)
    //             {
    //                 return item as Card;
    //             }
    //         }
    //     }

    //     return null;
    // }

    // private void Update()
    // {
    //     if (FindCard() != null)
    //     {
    //         cardToAddHack = FindCard();
    //     }
    // }
}
