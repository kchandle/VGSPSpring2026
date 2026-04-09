using System;

public static class InventoryEvents
{
    public static event Action OnCardHacked;
    
    public static void CardHacked()
    {
        if (OnCardHacked != null)
            OnCardHacked();
    }
}
