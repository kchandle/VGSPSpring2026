using System;

public class CardNotInDatabaseException : Exception
{
    public CardNotInDatabaseException() { }
    public CardNotInDatabaseException(string message) : base(message) { }
}

public class InventoryFullException : Exception
{
    public InventoryFullException() { }
    public InventoryFullException(string message) : base(message) { }
}

public class DeckFullException : Exception
{
    public DeckFullException() { }
    public DeckFullException(string message) : base(message) { }
}

public class CardNotInInventoryException : Exception
{
    public CardNotInInventoryException() { }
    public CardNotInInventoryException(string message) : base(message) { }
}
