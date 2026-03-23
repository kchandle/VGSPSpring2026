/* Author: DerjenigeUberMensch
 *
 * Contact Group 1 For help or questions relating to this script.
 */
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
public class Shop : MonoBehaviour
{
    // Events
    static public event Action PurchaseEvent;
    static public event Action FailPurchaseEvent;
    static public event Action SellEvent;
    public event Action StockUpdate;

    //Set in editor
    [SerializeField] private GameObject player; 
    public GameObject shopUI; 

    //Not set in editor
    /*[HideInInspector]*/ public List<ShopItem> stock = new();
    /*[HideInInspector]*/ public List<Card_SO> cachedSOs = new();

    private List<ShopItem> inventory = new();
    private List<InventoryCard> playerInv;

    //StockSize is how many items will currently be in shop
    private int _StockSize = -1;
    public int StockSize 
    { 
        get
        {   return this._StockSize;
        }

        set
        {
            if(stock.Count == value)
            {   return;
            }

            if(value < 0)
            {   
                Debug.LogWarning($"MaxCardsInStock was set to {value}");
                value = 0;
            }

            this._StockSize = value;

            // if the stock is too big shrink.
            while(stock.Count > this._StockSize)
            {   this.stock.RemoveAt(stock.Count - 1);
            }

            // if the stock is too small grow it.
            if(stock.Count < this._StockSize)
            {
                this.stock.AddRange(this.GenerateStock(this._StockSize, this.stock));
            }
        }
    }

    //Toggles the UI if true or false
    public bool IsShopOpenUI
    {
        // todo
        get
        {   return shopUI.activeSelf;
        }
        set
        {   if(value)
            {
                SaveSystem.Load(player);
            }
            shopUI.SetActive(value);
        }
    }

    //Gets the cardSos from the folder they are stored in path, which is where the programmers put the cardSOs
    private List<UnityEngine.Object> GetObjectsInPath(string path)
    {
        List<UnityEngine.Object> assets = new();

        string []guids = AssetDatabase.FindAssets("", new[] { path });

        foreach (string guid in guids)
        {
            path = AssetDatabase.GUIDToAssetPath(guid);
            UnityEngine.Object asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);

            if (asset != null)
            {   assets.Add(asset);
            }
        }

        return assets;
    }

    //Updates our inventory when it's changed
    private void OnInventoryChange(object unused1, EventArgs unused2)
    {
        playerInv = Inventory.InventoryList;
    }

    //Initializes inventory stuff
    void Start()
    {
        playerInv = Inventory.InventoryList;
        Inventory.inventoryChanged += OnInventoryChange;
    }

    // Opens the shop UI.
    public void OpenShop()
    {   this.IsShopOpenUI = true;
    }

    // Closes the shop UI.
    public void CloseShop()
    {   this.IsShopOpenUI = false;
    }

    // Checks whether you can buy a card.
    public bool CanBuyCard(ShopItem item)
    {
        int playerBalance = Inventory.Money;

        return item.PurchasePrice <= playerBalance;
    }

    // Check whether you can sell a card.
    public bool CanSellCard(ShopItem item)
    {
        // return true for now.
        return true;
    }

    // Purchases a card in the shop.
    //
    // RETURN: true on Succesful purchase.
    // RETURN: false on failed to purchase.
    public bool BuyCard(ShopItem item)
    {
        //If the card can't be bought
        if(!this.CanBuyCard(item))
        {   
            Shop.FailPurchaseEvent?.Invoke();

            return false;
        }

        //Make sure inventory isn't null
        if(playerInv == null)
        {   
            Shop.FailPurchaseEvent?.Invoke();

            return false;
        }

        //Make sure there is enough space in inventory
        if(playerInv.Count >= Inventory.InventorySize + 1)
        {
            Shop.FailPurchaseEvent?.Invoke();

            return false;
        }

        //Make sure you have enough money
        if(Inventory.Money < (int)item.PurchasePrice)
        {   
            Shop.FailPurchaseEvent?.Invoke();

            return false;
        }

        //Buy the card, remove money
        Inventory.Money -= (int)item.PurchasePrice;
        Inventory.AddCardToInventory(item.SO);
        SaveSystem.Save(player);
        Shop.PurchaseEvent?.Invoke();

        

        return true;
    }

    // Sells a card in the sthop.
    //
    // RETURN: true on Succesful sell.
    // RETURN: false on failed to sell 
    public bool SellCard(ShopItem item)
    {
        
        if(!this.CanSellCard(item))
        {   
            return false;
        }

        if(playerInv == null)
        {   
            return false;
        }

        //Remove the corresponding card in the inventory
        foreach(InventoryCard c in playerInv)
        {
            if(c.cardSO == item.SO)
            {   
                Inventory.RemoveCardFromInventory(c);
                Inventory.Money += (int)item.SellPrice;
                Shop.SellEvent?.Invoke();
                SaveSystem.Save(player);
                return true;
            }
        }

        

        return false;
    }

    //Generates a new random shop stock, ignores the cards in the exclude list
    private List<ShopItem> GenerateStock(int maxStockToGenerate, List<ShopItem> exclude = null) 
    { 
        //We will need a concrete way to differentiate between cards for the player and cards only for enemies.
        //For now, the stock consists of only Jolt and Shield
        if(cachedSOs.Count == 0)
        {   
            cachedSOs = this.GetObjectsInPath("Assets/Resources/Card_SO")
                .OfType<Card_SO>()
                .Where(s => s.displayName == "Jolt" || s.displayName == "Shield" && s.price != 0)
                .ToList();
        }

        List<ShopItem> generatedStock = new();

        foreach(Card_SO obj in this.cachedSOs)
        {
            if(maxStockToGenerate <= 0)
            {   break;
            }

            // exclude any in the exclude list.
            if(exclude != null && exclude.Any(exc => exc.SO == obj))
            {   continue;
            }

            
            ShopItem item = new();

            item.Init(obj);
            //print(item);

            generatedStock.Add(item);
            //print(generatedStock);
        }

        return generatedStock;
    }

    //replaces the shop stock with the passed in list
    public void ReplaceStock(ref List<ShopItem> list)
    {
        this.stock = list;
        StockUpdate?.Invoke();
    }
}