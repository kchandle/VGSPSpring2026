using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Shop Panel just displays the information of the card and allows the player to buy / sell it
public class ShopPanel : MonoBehaviour
{
    //Visual elements of the panel (all set in editor)
    [SerializeField] private Image cardImage;
    [SerializeField] private TMPro.TextMeshProUGUI cardName;
    [SerializeField] private TMPro.TextMeshProUGUI cardDesc;
    [SerializeField] private TMPro.TextMeshProUGUI cardValue;
    
    
    //other
    private ShopItem item;
    private Card_SO cardSO;

    public int cardAmount = 1;
    public TMP_Text totalCardAmount;


    private int cardCost;
    private int cardSellValue;
    
    [SerializeField] private Shop shop; //set in editor
    [SerializeField] private ShopUI shopUI; //set in editor

    //Change visual elements of panel. Called by the Onclick events of whichever shopItem was clicked
    public void UpdatePanel(ShopItem shopItem)
    {
        cardSO = shopItem.SO;
        item = shopItem;

        cardImage.sprite = cardSO.image;
        cardDesc.text = cardSO.description;
        cardName.text = cardSO.displayName;

        cardCost = cardSO.price;
        cardSellValue = cardSO.sellValue;

        cardValue.text = "Buy: " + cardCost + " currency.\nSell: " + cardSellValue + " currency.";
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
