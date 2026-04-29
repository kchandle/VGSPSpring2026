using UnityEngine;
using UnityEngine.UI;
using TMPro;

//could be expanded to add items in the future
[System.Serializable]
/*public enum ItemType
{
    NULL,

    CARD_SO,
    HACK_SO,

    OTHER
}*/

//Shop Panel just displays the information of the card and allows the player to buy / sell it
public class ShopPanel : MonoBehaviour
{
    //Visual elements of the panel (all set in editor)
    [SerializeField] private Image itemImage;
    [SerializeField] private TMPro.TextMeshProUGUI itemName;
    [SerializeField] private TMPro.TextMeshProUGUI itemDesc;
    [SerializeField] private TMPro.TextMeshProUGUI itemValue;
    
    
    //other
    private ItemType type;
    private ShopItem item;
    private Card_SO cardSO;
    private Hack_SO hackSO;

    public int cardAmount = 1;
    public TMP_Text totalCardAmount;


    private int itemCost;
    private int itemSellValue;
    
    [SerializeField] private Shop shop; //set in editor
    [SerializeField] private ShopUI shopUI; //set in editor

    //Change visual elements of panel. Called by the Onclick events of whichever shopItem was clicked
    public void UpdatePanel(ShopItem shopItem)
    {
        item = shopItem;
        type = item.itemType;

        if(shopItem.itemType == ItemType.CARD_SO)
        {
            cardSO = shopItem.SO_cardSO;
            hackSO = null;
        }
        else if(shopItem.SO_hackSO)
        {
            cardSO = null;
            hackSO = shopItem.SO_hackSO;
        }

        itemImage.sprite = item.Image;
        itemDesc.text = item.Description;
        itemName.text = item.DisplayName;

        itemCost = item.PurchasePrice;
        itemSellValue = item.SellPrice;

        itemValue.text = "Buy: " + itemCost + " currency.\nSell: " + itemSellValue + " currency.";
        totalCardAmount.text = cardAmount.ToString();

    }


    public void PlusCard()
    {
        cardAmount += 1;
        print("CardAmount: " + cardAmount);
        totalCardAmount.text = cardAmount.ToString();
    }

    public void MinusCard()
    {
        if (cardAmount >= 2)
        {
            cardAmount -= 1;
        }
        else
        {
            cardAmount = 1;
        }
        totalCardAmount.text = cardAmount.ToString();
    }



    //method called by the shop panel's Buy button onclick event
    public void ClickedBuyCard()
    {
        //print("Buying card...");
        for (int i = 0; i < cardAmount; i++)
        {
            if (Inventory.IsInventoryFull()) print("full: " + Inventory.Cardscount());

            if (!Inventory.IsInventoryFull())
            {
                print("no-full: " + Inventory.Cardscount());
                if (shop.BuyCard(item))
                {
                    shopUI.UpdateBuyMenu();
                    print("Card bought!");
                    shopUI.UpdateSellMenu();
                }
                else
                {
                    print("you're broke");
                }
            }
        }
    }

    //method called by the shop panel's Sell button onclick event 
    public void ClickedSellCard()
    {
        //print("Selling card...");
        if(shop.SellCard(item))
        {
            shopUI.UpdateSellMenu();
            print("Card sold!");
        }
        else
        {
            print("sell failed");
        }
    }

}
