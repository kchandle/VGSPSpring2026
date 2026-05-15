/* Author: DerjenigeUberMensch
 *
 * Contact Group 1 For help or questions relating to this script.
 */
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.EventSystems;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;




public class ShopUI : MonoBehaviour
{
    // ui elements
    private GameObject canvas;
    [SerializeField] private GameObject content; //set in editor
    [SerializeField] private GameObject sellContent; //set in editor
    [SerializeField] private TMPro.TextMeshProUGUI playerMoneyText; //set in editor
    [SerializeField] private GameObject shopSlotTemplate; //set in editor, is the lone cardShopTemplate object under IgnoreContent. This was more convenient than making a prefab.
    
    //Music Player
    [SerializeField] private MusicPlayer musicPlayer;

    //Ref to shop script, which controls stock and the core functionality of buying and selling cards.
    [SerializeField] private Shop shop = new();

    [Tooltip("Volume when not inside the Shop UI")]
    public float UnfocusVolume = .65f;
    [Tooltip("Volume when inside the Shop UI")]
    public float FocusVolume = .9f;

    //Just returns the UI Element under the mouse, only used in Update()
    public static GameObject GetUIElementUnderMouse()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return results.Count > 0 ? results[0].gameObject : null;
    }

    //Initializes key variables
    void Start()
    {
        Transform root = gameObject.transform;

        this.canvas = gameObject;

        this.musicPlayer = FindAnyObjectByType<MusicPlayer>();

        this.musicPlayer.Play();

        shop.StockSize = 10;

        CreateBuyMenu();
        CreateSellMenu();
    }

    
    //Updates musicPlayer to make the volume go down and up
    void Update()
    {
        GameObject element = ShopUI.GetUIElementUnderMouse();

        //Make the music quieter if mouse if over a ShopUI element
        if(element == null || !element.activeInHierarchy)
        {   
            this.musicPlayer.Volume = this.UnfocusVolume;
            return;
        }

        //Make the music louder if mouse if over a ShopUI element
        if(element.transform.IsChildOf(this.canvas.transform))
        {   
            this.musicPlayer.Volume = this.FocusVolume;
        }

        if(!element.transform.IsChildOf(this.content.transform))
        {   
            return;
        }

        // Element is part of content so that means its in the shop ui.
    }


    #region Buy menu
    //=====================Buying menu=====================//
    //Create list of shopSlotTemplates, This method should only be called at start
    public void CreateBuyMenu(List<ShopItem> exclude = null)
    {
        //print("CREATING BUY MENU");
        playerMoneyText.text = "$" + Inventory.Money;

        //Destroy any possible existing shop slots
        while (content.transform.childCount > 0)
        {
            DestroyImmediate(content.transform.GetChild(0).gameObject);
        }


        int i = 1;
        foreach(ShopItem item in shop.stock)
        {
            // exclude any in the exclude list.
            if( exclude != null && (exclude.Any(exc => exc.SO_cardSO == item.SO_cardSO)))// || exclude.Any(exc => exc.SO_hackSO == item.SO_hackSO)) )
            {   
                continue;
            }


            GameObject cardSlot = Instantiate(shopSlotTemplate, new Vector3(0,0,0), Quaternion.identity);
            cardSlot.transform.SetParent(content.transform, false);
            //
            cardSlot.GetComponent<ShopSlot>().Init(item);
            cardSlot.SetActive(true);
            cardSlot.gameObject.name = "Buy " + (i);
            i++;
        }

        UpdateBuyMenu();
    }

    //Gets cards from the shop's stock and creates slots using the cardShopTemplate prefab to display them 
    public void UpdateBuyMenu(List<ShopItem> exclude = null)
    { 
        //print("UPDATING BUY MENU");
        playerMoneyText.text = "$" + Inventory.Money;
        
        //Set all shop slots inactive to begin with
        //Regular for loops for indexing
        int i = 0;
        for(i = 0; i < content.transform.childCount; i++)
        {
            content.transform.GetChild(i).gameObject.SetActive(false);
        }

        //Update needed shop slots, leave any remaining ones inactive
        for(i = 0; i < shop.stock.Count; i++)
        {
            ShopItem item = shop.stock[i];
            content.transform.GetChild(i).gameObject.GetComponent<ShopSlot>().Init(item);
            content.transform.GetChild(i).gameObject.SetActive(true);
        }

        
    }
    #endregion
    

    //Only called by the test button OnClick event, can be removed when no longer needed for testing.
    public void GiveMoney() 
    {
        Inventory.Money += 1000;
        playerMoneyText.text = "$" + Inventory.Money;
        int i = 0;
        //print("Cards in your inventory: ");
        foreach(InventoryCard card in Inventory.InventoryList)
        {
            i++;
            //print($"Card {i}: {card.cardSO.displayName}");
        }
    }
    

    #region Sell menu
    //=====================Selling menu=====================//
    //Gets cards from the Inventory and creates slots using the cardShopTemplate prefab to display them
    //Note: Hacked cards currently can't be sold.
    public void CreateSellMenu(List<ShopItem> exclude = null)
    { 
        //print("CREATING SELL MENU");
        playerMoneyText.text = "$" + Inventory.Money;

        //Destroy any possible existing shop slots
        while (sellContent.transform.childCount > 0)
        {
            DestroyImmediate(sellContent.transform.GetChild(0).gameObject);
        }

        //Create enough shop slots in the sell menu for all cards and hacks in the player's inventory
        int i = 0;
        for(i = 0; i < Inventory.InventorySize + Inventory.HackInventorySize; i++)
        {
            GameObject cardSlot = Instantiate(shopSlotTemplate, new Vector3(0,0,0), Quaternion.identity);
            cardSlot.transform.SetParent(sellContent.transform, false);
            cardSlot.gameObject.name = "Sell " + (i+1);
        }

        UpdateSellMenu();
    }

    //Activates and deactivates shopSlotTemplates as needed according to the cards in the player's inventory
    public void UpdateSellMenu(List<ShopItem> exclude = null)
    {
        //print("UPDATING SELL MENU");
        playerMoneyText.text = "$" + Inventory.Money;

        //Set all shop slots inactive to begin with
        int i = 0;
        for(i = 0; i < sellContent.transform.childCount; i++)
        {
            sellContent.transform.GetChild(i).gameObject.SetActive(false);
        }

        print("cards sell");
        //Set shop slots that contain player Inventory cards active, leave others inactive
        for(i = 0; i < Inventory.Cardscount(); i++)
        {
            InventoryCard invCard = Inventory.InventoryList[i];

            // exclude any in the exclude list.
            if( exclude != null && exclude.Any(exc => exc.SO_cardSO == invCard.cardSO) )
            {   
                continue;
            }




            //Every card made has a default hack length of 2. Assuming adding a hack increases the lenght to 3, hacked cards will not display
            if(invCard.hacks.Length <= 2)
            {
                ShopItem item = new ShopItem();
                item.Init_cardSO(invCard.cardSO);

                sellContent.transform.GetChild(i).gameObject.GetComponent<ShopSlot>().Init(item);
                sellContent.transform.GetChild(i).gameObject.SetActive(true);
            }
        }

        

        //Set shop slots that contain hacks active, leave others inactive
        for(i = Inventory.Cardscount(); i < Inventory.Cardscount() + Inventory.HackInventory.Count; i++)
        {
            Hack_SO hack = Inventory.HackInventory[i - Inventory.InventoryList.Count];

            // exclude any in the exclude list.
            if( exclude != null && exclude.Any(exc => exc.SO_hackSO == hack) )
            {   
                continue;
            }

            ShopItem item = new ShopItem();
            item.Init_hackSO(hack);
        
            sellContent.transform.GetChild(i).gameObject.GetComponent<ShopSlot>().Init(item);
            sellContent.transform.GetChild(i).gameObject.SetActive(true);
            
        }
    }
    #endregion















    
    #region unused code graveyard
    //==================Code not currently in use==================//

    /*private GameObject scrollview;
    private GameObject viewport;
    private GameObject cardTemplate;*/

    //[SerializeField] private List<Card_SO> stock; //set in editor
    //[SerializeField] private List<(GameObject obj, ShopItem shopItem)> cards = new();

    /*this.cardTemplate = root.Find("CardShopTemplate")?.gameObject;
    this.scrollview = root.Find("Scroll View")?.gameObject;
    this.viewport = root.Find("Scroll View/Viewport")?.gameObject;*/
    //shop.StockUpdate += OnStockUpdate;


    /*//Adds cards to stock.
    public void AddCardSOToStock(Card_SO card, int amount = 1)
    {
        for(int i = 0; i < amount; i++)
        {
            stock.Add(card);
        }
    }

    //Removes card from stock. Currently used when a card is bought.
    public void RemoveCardSOFromStock(Card_SO card)
    {
        stock.Remove(card);
    }

    //v1
    public void UpdateBuyMenu(List<ShopItem> exclude = null)
    { 
        //Destroy old items to make way for new ones
        while (content.transform.childCount > 0)
        {
            DestroyImmediate(content.transform.GetChild(0).gameObject);
        }

         
        print("Generating stock");
        foreach(Card_SO card in stock)
        {
            print("stock: " + card.displayName);
            // exclude any in the exclude list.
            if(exclude != null && exclude.Any(exc => exc.SO == card))
            {   
                continue;
            }

            GameObject cardSlot = Instantiate(shopSlotTemplate, new Vector3(0,0,0), Quaternion.identity);
            cardSlot.transform.SetParent(content.transform, false);
            cardSlot.GetComponent<ShopItem>().Init(card);
            cardSlot.SetActive(true);
        }
        print("Finished generating stock");
    }*/


    //v2
    /*public void UpdateBuyMenu(List<ShopItem> exclude = null)
    { 
        //Destroy old items to make way for new ones
        while (content.transform.childCount > 0)
        {
            DestroyImmediate(content.transform.GetChild(0).gameObject);
        }

         
        //print("Generating stock");
        foreach(ShopItem item in shop.stock)
        {
            //print("stock: " + item.SO.displayName);
            // exclude any in the exclude list.
            if(exclude != null && exclude.Any(exc => exc.SO == item.SO))
            {   
                continue;
            }

            GameObject cardSlot = Instantiate(shopSlotTemplate, new Vector3(0,0,0), Quaternion.identity);
            cardSlot.transform.SetParent(content.transform, false);
            //cardSlot.GetComponent<ShopItem>().Init(item.SO);
            cardSlot.GetComponent<ShopSlot>().Init(item);
            cardSlot.SetActive(true);
        }
        //print("Finished generating stock");
    }*/


    //v1
    /*public void UpdateSellMenu(List<ShopItem> exclude = null)
    { 
        playerMoneyText.text = "$" + Inventory.Money;

        //Destroy all old items in the sell menu in order to create the new ones without issue
        while (sellContent.transform.childCount > 0)
        {
            //print("Destroying the sell children");
            DestroyImmediate(sellContent.transform.GetChild(0).gameObject);
        }


        //print("creating sell menu");
        foreach(InventoryCard invCard in Inventory.InventoryList)
        {
            // exclude any in the exclude list.
            if(exclude != null && exclude.Any(exc => exc.SO == invCard.cardSO))
            {   
                continue;
            }

            //print("Card: " + invCard.cardSO.displayName + ". Hack length: " + invCard.hacks.Length);


            //Every card made has a hack length of 2
            if(invCard.hacks.Length <= 2)
            {
                //print("creating " + invCard.cardSO.displayName);
                
                GameObject cardSlot = Instantiate(shopSlotTemplate, new Vector3(0,0,0), Quaternion.identity);
                cardSlot.transform.SetParent(sellContent.transform, false);

                ShopItem item = new ShopItem();
                item.Init(invCard.cardSO);

                cardSlot.GetComponent<ShopSlot>().Init(item);
                cardSlot.SetActive(true);
            }

        }
        //print("finished creating sell menu");

        //print("test: " + Inventory.InventoryList[1].cardSO.displayName);
    }*/




    //Unused code
    /*void OnStockUpdate()
    {
        // mark all cards as dirty unless we find them.
        List<int> dirty = Enumerable.Range(0, this.cards.Count).ToList();

        // try and find cards.
        foreach(ShopItem item in shop.stock)
        {
            int index = this.cards.FindIndex(tuple => tuple.shopItem == item);
            bool found = index != -1;

            if(found)
            {   dirty.Remove(index);
            }
            else
            {   
                GameObject clone = Instantiate(this.cardTemplate, this.content.transform.parent);

                this.cards.Add((clone, item));
            }
        }

        // sort to decending list.
        dirty.Sort((a, b) => b.CompareTo(a));

        foreach (int index in dirty)
        {
            if (index >= 0 && index < this.cards.Count)
            {   
                (GameObject obj, ShopItem unused) = this.cards[index];

                Destroy(obj);

                this.cards.RemoveAt(index);
            }
        }
    }*/
    #endregion
}