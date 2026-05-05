using UnityEngine;
using UnityEngine.UI;
using TMPro;



//Shop Panel just displays the information of the item and allows the player to buy / sell it
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

    public int itemAmount = 1;
    public TMP_Text totalItemAmount;


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
        totalItemAmount.text = itemAmount.ToString();

    }


    public void PlusItem()
    {
        itemAmount += 1;
        print("ItemAmount: " + itemAmount);
        totalItemAmount.text = itemAmount.ToString();
    }

    public void MinusItem()
    {
        if (itemAmount >= 2)
        {
            itemAmount -= 1;
        }
        else
        {
            itemAmount = 1;
        }
        totalItemAmount.text = itemAmount.ToString();
    }



    //method called by the shop panel's Buy button onclick event
    public void ClickedBuyItem()
    {
        //print("Buying card...");
        for (int i = 0; i < itemAmount; i++)
        {
            //Buying a CARD
            if(type == ItemType.CARD_SO)
            {
                if (Inventory.IsInventoryFull())
                {
                    print("full: " + Inventory.Cardscount());
                }

                if (!Inventory.IsInventoryFull())
                {
                    //print("no-full: " + Inventory.Cardscount());
                    if (shop.BuyItem(item))
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
            //Buying a HACK
            else if(type == ItemType.HACK_SO)
            {
                if(Inventory.HackInventory.Count > Inventory.HackInventorySize)
                {
                    print("full on hacks");
                }
                else
                {
                    if (shop.BuyItem(item))
                    {
                        shopUI.UpdateBuyMenu();
                        print("Hack bought!");
                        shopUI.UpdateSellMenu();
                    }
                    else
                    {
                        print("you're broke");
                    }
                }
            }
            
        }
    }

    //method called by the shop panel's Sell button onclick event 
    public void ClickedSellItem()
    {
        //print("Selling card...");
        if(shop.SellItem(item))
        {
            shopUI.UpdateSellMenu();
            //print("Card / Hack sold!");
            print(type + " sold!");
        }
        else
        {
            print("sell failed");
        }
    }

}
