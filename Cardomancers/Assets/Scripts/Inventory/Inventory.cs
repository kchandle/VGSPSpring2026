using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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
    #endregion

    #region Limiting Variables
    private static int inventorySize;
    private static int deckSize;
    private static int hackInventorySize;
    #endregion
    
    // Raised whenever inventory, deck, or hack inventory is changed
    public static EventHandler inventoryChanged;

    #region Properties
    public static Dictionary<string, Card_SO> CardsDatabase
    {
        set { cardsDatabase ??= value; }
        get => cardsDatabase;
    }

    public static Dictionary<string, Hack_SO> HacksDatabase
    {
        set { hacksDatabase ??= value; }
        get => hacksDatabase;
    }

    public static List<InventoryCard> InventoryList
    {
        get => inventory;
    }

    public static List<InventoryCard> Deck
    {
        get => deck;
    }

    public static List<Hack_SO> HackInventory
    {
        get => hackInventory;
    }
    #endregion
    
    #region Add to card to Inventory

    public static void AddCardToInventory(Card_SO card)
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

    public static void AddCardToInventory(Card_SO card, int numberOfCards)
    {
        while (numberOfCards > 0)
        {
            try
            {
                AddCardToInventory(card);
                numberOfCards--;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public static void AddCardToInventory(InventoryCard card)
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

    public static void AddCardToInventory(InventoryCard card, int numberOfCards)
    {
        while (numberOfCards > 0)
        {
            try
            {
                AddCardToInventory(card);
                numberOfCards--;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
    #endregion

    #region Deck

    public static void AddCardToDeck(InventoryCard card)
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
    }

    public static void AddCardToDeck(InventoryCard card, int numberOfCards)
    {
        while (numberOfCards > 0)
        {
            try
            {
                AddCardToDeck(card);
                numberOfCards--;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }

    public static void RemoveCardFromDeck(InventoryCard card)
    {
        if (!deck.Contains(card))
        {
            
        }
    }

    #endregion
}
