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

    [SerializeField] private TMP_Text CardAmountText;

    //other
    private ShopItem item;
    private Card_SO cardSO;

    public int cardCost;
    private int cardSellValue;

    int cardAmount = 1;


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

    }

    void Update()
    {
       // CardAmountText = GetComponent<TMP_Text>();
        CardAmountText.text = (cardAmount.ToString());;
       
    }


    public void AddCard()
    {
        cardAmount++;
    }
    public void RemoveCard()
    {
        if(cardAmount > 1)
        {
            cardAmount--; 
        }
    }


    //method called by the shop panel's Buy button onclick event
    public void ClickedBuyCard()
    {
        for (int i = cardAmount;  i > 0; i--)
        {
            //print("Buying card...");
            // Debug.unityLogger.logEnabled = false;
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
           // Debug.unityLogger.logEnabled = true;
        }


    }

    public void SetCardAmountToOne()
    {
        cardAmount = 1;
    }

    //method called by the shop panel's Sell button onclick event 
    public void ClickedSellCard()
    {
     
        //print("Selling card...");
        if (shop.SellCard(item))
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
