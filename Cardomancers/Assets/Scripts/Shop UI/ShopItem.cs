/* Author: DerjenigeUberMensch
 *
 * Contact Group 1 For help or questions relating to this script.
 */
using UnityEngine;
using UnityEngine.UI;

public enum ItemType
{
    NULL,

    CARD_SO,
    HACK_SO,

    OTHER
}

public class ShopItem : ScriptableObject
{
    //Unused for now
    public int Stock { get; set; } = 0;



    #region Item Type Info

    //Refer to this to tell if the shopItem is a hack or card
    [SerializeField] private ItemType _itemType;
    public ItemType itemType
    {
        get
        {
            if( !(cardSO == null) )
            {   
                return ItemType.CARD_SO;
            }
            else if( !(hackSO == null) )
            {   
                return ItemType.HACK_SO;
            }
            else
            {
                Debug.LogError("Shop Item cardSO and hackSO are null");
                return ItemType.NULL; //does nothing
            }
        }
        set
        {
            if( !(cardSO == null) )
            {   
                _itemType = value;
            }
            else if( !(hackSO == null) )
            {   
                _itemType = value;
            }
            else
            {
                Debug.LogError("Shop Item cardSO and hackSO are null");
                return;
            }
        }
    }

    //The following are for the cardSO version of a shopItem.
    [SerializeField] private Card_SO cardSO;
    public CardType Type_cardSO
    {
        get
        {   
            if(cardSO == null)
            {   
                Debug.LogError("Shop Item cardSO is null");
                return CardType.NULL;
            }
            return cardSO.CardType;
        }
    }
    public Card_SO SO_cardSO
    {
        get
        {   
            return cardSO;
        }
    }
    public void Init_cardSO(Card_SO cardSO) 
    {
        if(cardSO == null)
        {   
            Debug.LogError("Argument \"cardSO\" is null");
            return;
        }
        this.cardSO = cardSO;
        this.Image = cardSO.image;
        this.itemType = ItemType.CARD_SO;
    } 



    //The following is for the hackSO version of a shopItem
    [SerializeField] private Hack_SO hackSO;
    public Hack_SO SO_hackSO
    {
        get
        {   
            return hackSO;
        }
    }
    public void Init_hackSO(Hack_SO hackSO) 
    {
        if(hackSO == null)
        {   
            Debug.LogError("Argument \"hackSO\" is null");
            return;
        }
        this.hackSO = hackSO;
        this.Image = hackSO.image;
        this.itemType = ItemType.HACK_SO;
    } 
    #endregion







    #region Item Data
    //All of the following retrieves data from either the cardSO or hackSO

    public int SellPrice
    { 
        get
        {   
            if( itemType == ItemType.CARD_SO )
            {   
                return cardSO.sellValue; 
            }
            else if ( itemType == ItemType.HACK_SO )
            {
                return hackSO.sellValue;
            }
            else
            {
                Debug.LogError("Shop Item cardSO and hackSO are null");
                return 0;
            }
  
        }
        set
        {   
            if( itemType == ItemType.CARD_SO )
            {   
                cardSO.sellValue = (int)value; 
            }
            else if( itemType == ItemType.HACK_SO )
            {   
                hackSO.sellValue = (int)value; 
            }

            else
            {
                Debug.LogError("Shop Item cardSO and hackSO area null");
                return;
            }

        }
    }

    public int PurchasePrice
    { 
        get
        {   

            if( itemType == ItemType.CARD_SO )
            {   
                return cardSO.price; 
            }
            else if ( itemType == ItemType.HACK_SO )
            {
                return hackSO.Price;
            }
            else
            {
                Debug.LogError("Shop Item cardSO and hackSO are null");
                return 0;
            }
        }

        set
        {   
            if( itemType == ItemType.CARD_SO )
            {   
                cardSO.price = (int)value; 
            }
            else if ( itemType == ItemType.HACK_SO )
            {
                hackSO.Price = (int)value;
            }
            else
            {
                Debug.LogError("Shop Item cardSO and hackSO are null");
                return;
            }
        } 
    }

    public string DisplayName
    {   
        get
        {   
            if( itemType == ItemType.CARD_SO )
            {   
                return cardSO.displayName; 
            }
            else if ( itemType == ItemType.HACK_SO )
            {
                return hackSO.displayName;
            }
            else
            {
                Debug.LogError("Shop Item cardSO and hackSO are null");
                return "";
            }
        }
        set
        {   
            if( itemType == ItemType.CARD_SO )
            {   
                cardSO.displayName = value;
            }
            else if ( itemType == ItemType.HACK_SO )
            {
                hackSO.displayName = value;
            }
            else
            {
                Debug.LogError("Shop Item cardSO and hackSO are null");
                return;
            }
        }
    }

    public string Description
    {
        get
        {   
            if( itemType == ItemType.CARD_SO )
            {   
                return cardSO.description;
            }
            else if ( itemType == ItemType.HACK_SO )
            {
                return hackSO.description;
            }
            else
            {
                Debug.LogError("Shop Item cardSO and hackSO are null");
                return "";
            }
        }
        set
        {  
            if( itemType == ItemType.CARD_SO )
            {   
                cardSO.description = value;
            }
            else if ( itemType == ItemType.HACK_SO )
            {
                hackSO.description = value;
            }
            else
            {
                Debug.LogError("Shop Item cardSO and hackSO are null");
                return;
            }
        }
    }

    public Sprite Image
    {   
        get
        {   
            if( itemType == ItemType.CARD_SO )
            {   
                return cardSO.image;
            }
            else if ( itemType == ItemType.HACK_SO )
            {
                return hackSO.image;
            }
            else
            {
                Debug.LogError("Shop Item cardSO and hackSO are null");
                return null;
            }
        }
        set
        {   
            if( itemType == ItemType.CARD_SO )
            {   
                cardSO.image = value;
            }
            else if ( itemType == ItemType.HACK_SO )
            {
                hackSO.image = value;
            }
            else
            {
                Debug.LogError("Shop Item cardSO and hackSO are null");
                return;
            }
        }
    }
    #endregion
}