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

public class ShopUI : MonoBehaviour
{
    // ui elements
    private GameObject canvas;
    private GameObject scrollview;
    private GameObject viewport;
    private GameObject cardTemplate;
    [SerializeField] private GameObject content; //set in editor
    [SerializeField] private GameObject sell_content; //set in editor
    //[SerializeField] private ShopPanel shopPanel; //set in editor
    [SerializeField] private GameObject cardShopPrefab; //set in editor, is the lone cardShopTemplate object under IgnoreContent. This was more convenient than making a prefab.
    private MusicPlayer musicPlayer;

    [SerializeField] private Shop shop = new();
    [SerializeField] private List<Card_SO> stock; //set in editor

    [SerializeField] private List<(GameObject obj, ShopItem shopItem)> cards = new();

    [Tooltip("Volume when not inside the Shop UI")]
    public float UnfocusVolume = .65f;
    [Tooltip("Volume when inside the Shop UI")]
    public float FocusVolume = .9f;

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

    private int _ContentChildCount = 0;
    private int ContentChildCount
    {
        get
        {   return _ContentChildCount;
        }

        set
        {
            if(_ContentChildCount == value)
            {   return;
            }

            _ContentChildCount = value;
        }
    }

    //private void 

    void Start()
    {
        Transform root = gameObject.transform;

        //The objects are null when referred to in this way
        this.canvas = gameObject;
        this.cardTemplate = root.Find("CardShopTemplate")?.gameObject;
        this.scrollview = root.Find("Scroll View")?.gameObject;
        this.viewport = root.Find("Scroll View/Viewport")?.gameObject;
        //this.content = root.Find("Scroll View/Viewport/Content")?.gameObject;
        //this.sell_content = root.Find("Scroll View/Viewport/Content(Sell)")?.gameObject; //*******
        this.musicPlayer = gameObject.GetComponent<MusicPlayer>();

        if(this.content)
        {   
            ContentChildCount = this.content.transform.childCount;
        }

        this.musicPlayer.Play();

        shop.StockUpdate += OnStockUpdate;

        shop.StockSize = 10;
        UpdateBuyMenu();
        UpdateSellMenu();
    }

    void OnStockUpdate()
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
    }

    void Update()
    {
        /*GameObject element = ShopUI.GetUIElementUnderMouse();

        if(element == null || !element.activeInHierarchy)
        {   
            this.musicPlayer.Volume = this.UnfocusVolume;
            return;
        }

        if(element.transform.IsChildOf(this.canvas.transform))
        {   this.musicPlayer.Volume = this.FocusVolume;
        }

        if(!element.transform.IsChildOf(this.content.transform))
        {   return;
        }

        // Element is part of content so that means its in the shop ui.

        

        ContentChildCount = this.content.transform.childCount;*/
    }




    //******
    //Adds cards to stock. Currently unused.
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


    //Gets cards from the shop's stock and creates slots using the cardShopTemplate prefab to display them
    /*public void UpdateBuyMenu(List<ShopItem> exclude = null)
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

            GameObject cardSlot = Instantiate(cardShopPrefab, new Vector3(0,0,0), Quaternion.identity);
            cardSlot.transform.SetParent(content.transform, false);
            cardSlot.GetComponent<ShopItem>().Init(card);
            cardSlot.SetActive(true);
        }
        print("Finished generating stock");
    }*/
    
    public void UpdateBuyMenu(List<ShopItem> exclude = null)
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

            GameObject cardSlot = Instantiate(cardShopPrefab, new Vector3(0,0,0), Quaternion.identity);
            cardSlot.transform.SetParent(content.transform, false);
            //cardSlot.GetComponent<ShopItem>().Init(item.SO);
            cardSlot.GetComponent<ShopSlot>().Init(item);
            cardSlot.SetActive(true);
        }
        //print("Finished generating stock");
    }


    //Gets cards from the Inventory and creates slots using the cardShopTemplate prefab to display them
    public void UpdateSellMenu(List<ShopItem> exclude = null)
    { 
        //Destroy all old items in the sell menu in order to create the new ones without issue
        while (sell_content.transform.childCount > 0)
        {
            //print("Destroying the sell children");
            DestroyImmediate(sell_content.transform.GetChild(0).gameObject);
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


            //For some reason, every card made has a hack length of 2
            if(invCard.hacks.Length <= 2)
            {
                //print("creating " + invCard.cardSO.displayName);
                
                GameObject cardSlot = Instantiate(cardShopPrefab, new Vector3(0,0,0), Quaternion.identity);
                cardSlot.transform.SetParent(sell_content.transform, false);

                ShopItem item = new ShopItem();
                item.Init(invCard.cardSO);

                cardSlot.GetComponent<ShopSlot>().Init(item);
                cardSlot.SetActive(true);
            }

        }
        //print("finished creating sell menu");

        //print("test: " + Inventory.InventoryList[1].cardSO.displayName);
    }
}