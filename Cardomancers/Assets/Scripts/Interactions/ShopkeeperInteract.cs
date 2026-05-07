using UnityEngine;

public class ShopkeeperInteract : MonoBehaviour
{
    Shop shop;
    private void Awake()
    {
        shop = FindFirstObjectByType<Shop>(FindObjectsInactive.Include);
    }

    public void OnInteract()
    {
        if (shop.IsShopOpenUI) 
        {
            print("do nothing");
            shop.CloseShop();
        }
        else 
        {
            print("do something");
            shop.OpenShop();
        }
    }
}