using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
{
    [SerializeField] public  ShopItem shopItem; //NOT set in editor
    [SerializeField] private ShopPanel shopPanel; //set in editor
    [SerializeField] private Image image; //set in editor

    public void Init(ShopItem item)
    {
        shopItem = item;
        image.sprite = shopItem.SO.image;
    }

    public void Init(Card_SO so)
    {
        shopItem.Init(so);
        image.sprite = shopItem.SO.image;
    }

    //called when the object is clicked on 
    public void UpdateShopPanel()
    {
        shopPanel.UpdatePanel(shopItem);
    }
}
