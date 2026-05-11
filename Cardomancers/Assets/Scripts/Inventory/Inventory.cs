using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using InventoryExceptions;
using Unity.VisualScripting.FullSerializer;
using Random = System.Random;
using TMPro;

/// <summary>
/// Static class that manages the player's inventory, deck, hacks, and money.
/// Provides methods for adding/removing cards and hacks, as well as hacking cards.
/// </summary>
public static class Inventory
{
    #region Collections
    // Enforce good file organization 
    private static Dictionary<string, Card_SO> cardsDatabase;
    private static Dictionary<string, Hack_SO> hacksDatabase;
    
    // Inventory, contains all cards the player currently has
    private static List<InventoryCard> inventory = new List<InventoryCard>();
    // Deck, contains all cards the player can play in battle
    private static List<InventoryCard> deck = new List<InventoryCard>();
    // Hack Inventory, contains all the hacks the player has yet to apply to a card
    private static List<Hack_SO> hackInventory = new  List<Hack_SO>();
    
    private static PlayItem hackSlot;
    private static Card cardSlot;
    #endregion

    #region Limiting Variables
    private static int inventorySize = 10;
    private static int deckSize = 5;
    private static int hackInventorySize = 50;
    #endregion
    
    /// <summary>
    /// Raised whenever the inventory, deck, or hack inventory is modified.
    /// </summary>
    public static EventHandler inventoryChanged;

    /// <summary>
    /// The amount of money the player currently has.
    /// </summary>
    private static int money;

    #region Properties
    /// <summary>
    /// Database of all available cards, keyed by their name.
    /// </summary>
    public static Dictionary<string, Card_SO> CardsDatabase
    {
        set { cardsDatabase ??= value; }
        get => cardsDatabase;
    }

    /// <summary>
    /// Database of all available hacks, keyed by their name.
    /// </summary>
    public static Dictionary<string, Hack_SO> HacksDatabase
    {
        set { hacksDatabase ??= value; }
        get => hacksDatabase;
    }

    /// <summary>
    /// List of all cards currently in the player's inventory.
    /// </summary>
    public static List<InventoryCard> InventoryList
    {
        get => inventory;
        set => inventory = value;
    }
    
    
    /// <summary>
    /// List of cards in the hack creator.
    /// </summary>
    public static Card CardSlot
    {
        get => cardSlot;
        set => cardSlot = value;
    }
    
    public static PlayItem HackSlot
    {
        get => hackSlot;
        set => hackSlot = value;
    }
    
    /// <summary>
    /// List of cards currently in the player's active deck.
    /// </summary>
    public static List<InventoryCard> Deck
    {
        get => deck;
        set =>  deck = value;
    }

    /// <summary>
    /// List of hacks currently in the player's inventory.
    /// </summary>
    public static List<Hack_SO> HackInventory
    {
        get => hackInventory;
        set => hackInventory = value;
    }

    /// <summary>
    /// Gets or sets the player's current money.
    /// </summary>
    public static int Money
    {
        get => money;
        set
        {
            money = value;
            TMP_Text t = GameObject.FindWithTag("MoneyUI")?.transform.GetComponentInChildren<TMP_Text>();

            if (t != null)
            {   t.text = money.ToString();
            }
        }
    }

    public static int InventorySize
    {
        get => inventorySize;
        set => inventorySize = value;
    }

    public static int DeckSize
    {
        get => deckSize;
        set => deckSize = value;
    }

    public static int HackInventorySize
    {
        get => hackInventorySize;
        set => hackInventorySize = value;
    }
    #endregion
    
    #region Inventory Management

    /// <summary>
    /// Adds a card to the player's inventory based on a Card ScriptableObject.
    /// </summary>
    /// <param name="card">The Card_SO to add.</param>
    /// <exception cref="CardNotInDatabaseException">Thrown if the card is not in the database.</exception>
    /// <exception cref="InventoryFullException">Thrown if the inventory is already full.</exception>
    public static bool AddCardToInventory(Card_SO card)
    {
        if (cardsDatabase == null || !cardsDatabase.ContainsKey(card.name))
        {
            throw new CardNotInDatabaseException($"Card \"{card.name}\" not found in the database.");
        }
        if (inventory.Count >= inventorySize)
        {
            throw new InventoryFullException("Inventory is full.");
        }
        InventoryCard newCard = new InventoryCard(card);
            
        inventory.Add(newCard);

        inventoryChanged?.Invoke(null, EventArgs.Empty);
        //AddCardToDeck(newCard);
        return true;
    }

    public static void DeleteNullInInventory()
    {
        InventoryList.RemoveAll(card => card.cardSO == null);
        Debug.Log("Deleted null Cards. Remaining Cards: " + InventoryList.Count);

        HackInventory.RemoveAll(hack => hack == null);
        Debug.Log("Deleted null Hacls. Remaining Hacks: " + HackInventory.Count);
    }

    public static int Cardscount()
    {
        return inventory.Count;
    }

    public static bool IsInventoryFull()
    {
        return inventory.Count >= inventorySize;
    }

    /// <summary>
    /// Adds multiple copies of a card to the inventory.
    /// </summary>
    /// <param name="card">The Card_SO to add.</param>
    /// <param name="numberOfCards">The number of copies to add.</param>
    /// <exception cref="CardNotInDatabaseException">Thrown if the card is not in the database.</exception>
    public static bool AddCardToInventory(Card_SO card, int numberOfCards)
    {
        void InternalAdd()
        {
            if (cardsDatabase == null || !cardsDatabase.ContainsKey(card.name))
            {
                throw new CardNotInDatabaseException($"Card \"{card.name}\" not found in the database.");
            }
            if (inventory.Count >= inventorySize)
            {
                throw new InventoryFullException("Inventory is full.");
            }
            InventoryCard newCard = new InventoryCard(card);
            
            inventory.Add(newCard);
        }

        while (numberOfCards > 0)
        {
            try
            {
                InternalAdd();
                numberOfCards--;
            }
            catch (CardNotInDatabaseException e)
            {
                Debug.LogError(e);
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        inventoryChanged?.Invoke(null, EventArgs.Empty);
        return true;
    }

    /// <summary>
    /// Adds an existing InventoryCard instance to the inventory.
    /// </summary>
    /// <param name="card">The InventoryCard to add.</param>
    /// <exception cref="CardNotInDatabaseException">Thrown if the card's SO is not in the database.</exception>
    /// <exception cref="InventoryFullException">Thrown if the inventory is full.</exception>
    public static bool AddCardToInventory(InventoryCard card)
    {
            if (cardsDatabase == null || !cardsDatabase.ContainsKey(card.cardSO.name))
            {
                throw new CardNotInDatabaseException($"Card \"{card.cardSO.name}\" not found in the database.");
            }
            if (inventory.Count >= inventorySize)
            {
                throw new InventoryFullException("Inventory is full.");
            }
            
            
            inventory.Add(card);

        inventoryChanged?.Invoke(null, EventArgs.Empty);
        return true;
    }

    /// <summary>
    /// Adds multiple instances of an InventoryCard to the inventory.
    /// </summary>
    /// <param name="card">The InventoryCard instance to add.</param>
    /// <param name="numberOfCards">The number of times to add the card.</param>
    public static bool AddCardToInventory(InventoryCard card, int numberOfCards)
    {
        void InternalAdd()
        {
            if (cardsDatabase == null || !cardsDatabase.ContainsKey(card.cardSO.name))
            {
                throw new CardNotInDatabaseException($"Card \"{card.cardSO.name}\" not found in the database.");
            }
            if (inventory.Count >= inventorySize)
            {
                throw new InventoryFullException("Inventory is full.");
            }
            
            inventory.Add(card);
        }

        while (numberOfCards > 0)
        {
            try
            {
                InternalAdd();
                numberOfCards--;
            }
            catch (CardNotInDatabaseException e)
            {
                Debug.LogError(e);
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        inventoryChanged?.Invoke(null, EventArgs.Empty);
        return true;
    }

    /// <summary>
    /// Removes a card from the inventory and the deck if it's currently in it.
    /// </summary>
    /// <param name="card">The InventoryCard to remove.</param>
    /// <exception cref="CardNotInInventoryException">Thrown if the card is not in the inventory.</exception>
    /// <exception cref="CardNotInDatabaseException">Thrown if the card's SO is not in the database.</exception>
    /// <exception cref="CardNotInDeckException">Thrown if the card is in the deck list but not found when attempting to remove.</exception>
    public static void RemoveCardFromInventory(InventoryCard card)
    {
        if (!inventory.Contains(card))
        {
            throw new CardNotInInventoryException("Card not detected in in inventory, so it cannot be removed");
        }
        if (!cardsDatabase.ContainsKey(card.cardSO.name))
        {
            throw new CardNotInDatabaseException($"Card with card SO {card.cardSO.name} that is attempting to remove from inventory is not in the database.");
        }
        
        inventory.Remove(card);
        if (deck.Contains((card)))
        {
            if (!deck.Contains(card))
            {
                throw new CardNotInDeckException("The card that is being removed from the deck is not detected in the deck");
            }
            deck.Remove(card);
        }
        inventoryChanged?.Invoke(null, EventArgs.Empty);
    }

    #endregion

    #region Deck Management
    /// <summary>
    /// Adds a card from the inventory to the active deck.
    /// </summary>
    /// <param name="card">The InventoryCard to add.</param>
    /// <exception cref="DeckFullException">Thrown if the deck is full.</exception>
    /// <exception cref="CardNotInInventoryException">Thrown if the card is not in the inventory.</exception>
    /// <exception cref="CardNotInDatabaseException">Thrown if the card's SO is not in the database.</exception>
    public static bool AddCardToDeck(InventoryCard card)
    {
        if (deck.Count >= deckSize)
        {
            throw new DeckFullException("Deck is full");
        }
        if (!inventory.Contains(card))
        {
            throw new CardNotInInventoryException($"The card {card} was not found in the inventory.");
        }
        if (!cardsDatabase.ContainsKey(card.cardSO.name))
        {
            throw new CardNotInDatabaseException($"The card {card.cardSO.name} was not found in the database.");
        }
        
        deck.Add(card);
        inventoryChanged?.Invoke(null, EventArgs.Empty);
        return true;
    }

    /// <summary>
    /// Removes a card from the active deck.
    /// </summary>
    /// <param name="card">The InventoryCard to remove.</param>
    /// <exception cref="CardNotInDeckException">Thrown if the card is not in the deck.</exception>
    public static void RemoveCardFromDeck(InventoryCard card)
    {
        if (!deck.Contains(card))
        {
            throw new CardNotInDeckException("The card that is being removed from the deck is not detected in the deck");
        }
        deck.Remove(card);
        inventoryChanged?.Invoke(null, EventArgs.Empty);
    }

    /// <summary>
    ///  Randomizes the order of a list of inventory cards
    /// </summary>
    /// <param name="input"> The list of inventory cards to be randomized</param>
    /// <returns>A list of inventory cards with random order</returns>
    public static List<InventoryCard> Shuffle(List<InventoryCard> input)
    {
        return ShuffleList.Shuffle(input);
    }
    #endregion

    #region Hacking and Hacks Management

    /// <summary>
    /// Adds a hack to the hack inventory.
    /// </summary>
    /// <param name="hack">The Hack_SO to add.</param>
    /// <exception cref="HackNotInDatabaseException">Thrown if the hack is not in the database.</exception>
    /// <exception cref="HackInventoryFullException">Thrown if the hack inventory is full.</exception>
    public static void AddHackToInventory(Hack_SO hack)
    {
        void InternalAdd()
        {
            if (!hacksDatabase.ContainsKey(hack.name))
            {
                throw new HackNotInDatabaseException($"The hack {hack.name} was not found in the database.");
            }

            if (hackInventory.Count >= hackInventorySize)
            {
                throw new HackInventoryFullException("Hack inventory is full.");
            }
            
            hackInventory.Add(hack);
        }

        InternalAdd();
        inventoryChanged?.Invoke(null, EventArgs.Empty);
    }

    /// <summary>
    /// Adds multiple copies of a hack to the hack inventory.
    /// </summary>
    /// <param name="hack">The Hack_SO to add.</param>
    /// <param name="numberOfHacks">The number of copies to add.</param>
    public static void AddHackToInventory(Hack_SO hack, int numberOfHacks)
    {
        void InternalAdd()
        {
            if (!hacksDatabase.ContainsKey(hack.name))
            {
                throw new HackNotInDatabaseException($"The hack {hack.name} was not found in the database.");
            }

            if (hackInventory.Count >= hackInventorySize)
            {
                throw new HackInventoryFullException("Hack inventory is full.");
            }
            
            hackInventory.Add(hack);
        }

        while (numberOfHacks > 0)
        {
            try
            {
                InternalAdd();
                numberOfHacks--;
            }
            catch (HackNotInDatabaseException e)
            {
                Debug.LogError(e);
                throw;
            }
            catch (Exception)
            {
                throw;
            }
        }
        inventoryChanged?.Invoke(null, EventArgs.Empty);
    }

    /// <summary>
    /// Removes a hack from the hack inventory.
    /// </summary>
    /// <param name="hack">The Hack_SO to remove.</param>
    /// <exception cref="HackNotInInventoryException">Thrown if the hack is not in the inventory.</exception>
    public static void RemoveHackFromInventory(Hack_SO hack)
    {
        if (!hackInventory.Contains(hack))
        {
            throw new HackNotInInventoryException($"The hack {hack.name} was not found in the inventory.");
        }
        hackInventory.Remove(hack);
        inventoryChanged?.Invoke(null, EventArgs.Empty);
    }

    /// <summary>
    /// Applies a hack to a specific card in the inventory.
    /// </summary>
    /// <param name="card">The card to be hacked.</param>
    /// <param name="hack">The hack to apply.</param>
    /// <exception cref="CardNotInInventoryException">Thrown if the card is not in the inventory.</exception>
    /// <exception cref="HackNotInInventoryException">Thrown if the hack is not in the inventory.</exception>
    /// <exception cref="CardHackLimitReachedException">Thrown if the card has already reached its hack limit.</exception>
    public static void HackCard()
    {
        Card card = cardSlot;
        Hack_SO hack = ((InventoryHack)hackSlot).HackSO;
        
        RemoveCardFromInventory(card.inventoryCard);
        
        card.AddHackToCard(hack);
        
        Debug.Log("hack added, deleting old hack");
        // Remove from inventory logic
        InventoryUIHandler invUIHand =
            (InventoryUIHandler)UnityEngine.Object.FindAnyObjectByType(typeof(InventoryUIHandler));
        invUIHand.hackCombinePlayspace.DestroyPlayItem(hackSlot);
        
        RemoveHackFromInventory(hack);
        
        AddCardToInventory(card.inventoryCard);
        
        InventoryEvents.CardHacked();
        
        inventoryChanged?.Invoke(null, EventArgs.Empty);
    }
    #endregion
}
