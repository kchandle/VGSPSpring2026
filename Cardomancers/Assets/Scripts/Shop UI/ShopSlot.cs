using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] public  ShopItem shopItem; //NOT set in editor, set when an object with this script is created
    [SerializeField] private ShopPanel shopPanel; //set in editor
    [SerializeField] private Image image; //set in editor

    //Passes in the ShopItem SO, and by proxy the card SO, this slot in the shop represents
    public void Init(ShopItem item)
    {
        shopItem = item;
        image.sprite = shopItem.Image;  
    }

    //Unused
    public void Init(Card_SO so)
    {
        shopItem.Init_cardSO(so);
        image.sprite = shopItem.SO_cardSO.image;
    }

    //Called when the object is clicked on  
    public void UpdateShopPanel()
    {
        shopPanel.UpdatePanel(shopItem);
    }
}
