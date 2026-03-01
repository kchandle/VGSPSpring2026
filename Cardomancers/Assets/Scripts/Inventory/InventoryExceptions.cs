using System;

/// <summary>
/// Exception thrown when a card is not found in the card database.
/// </summary>
public class CardNotInDatabaseException : Exception
{
    public CardNotInDatabaseException() { }
    public CardNotInDatabaseException(string message) : base(message) { }
}

/// <summary>
/// Exception thrown when attempting to add a card to the inventory and it is already at maximum capacity.
/// </summary>
public class InventoryFullException : Exception
{
    public InventoryFullException() { }
    public InventoryFullException(string message) : base(message) { }
}

/// <summary>
/// Exception thrown when attempting to add a card to the deck and it is already at maximum capacity.
/// </summary>
public class DeckFullException : Exception
{
    public DeckFullException() { }
    public DeckFullException(string message) : base(message) { }
}

/// <summary>
/// Exception thrown when an operation is attempted on a card that is not in the inventory.
/// </summary>
public class CardNotInInventoryException : Exception
{
    public CardNotInInventoryException() { }
    public CardNotInInventoryException(string message) : base(message) { }
}

/// <summary>
/// Exception thrown when an operation is attempted on a card that is not in the deck.
/// </summary>
public class CardNotInDeckException : Exception
{
    public CardNotInDeckException() { }

    public CardNotInDeckException(string message) : base(message) { }
}

/// <summary>
/// Exception thrown when a hack is not found in the hack database.
/// </summary>
public class HackNotInDatabaseException : Exception
{
    public HackNotInDatabaseException() { }

    public HackNotInDatabaseException(string message) : base(message) { }
}

/// <summary>
/// Exception thrown when attempting to add a hack to the hack inventory and it is already at maximum capacity.
/// </summary>
public class HackInventoryFullException : Exception
{
    public HackInventoryFullException() { }

    public HackInventoryFullException(string message) : base(message) { }
}

/// <summary>
/// Exception thrown when an operation is attempted on a hack that is not in the hack inventory.
/// </summary>
public class HackNotInInventoryException : Exception
{
    public HackNotInInventoryException() { }

    public HackNotInInventoryException(string message) : base(message) { }
}

/// <summary>
/// Exception thrown when attempting to apply a hack to a card that has already reached its maximum number of hacks.
/// </summary>
public class CardHackLimitReachedException : Exception
{
    public CardHackLimitReachedException() { }

    public CardHackLimitReachedException(string message) : base(message) { }
}