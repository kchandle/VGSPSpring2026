using UnityEngine;
using static GameStateScript;

public class ShopkeeperInteract : MonoBehaviour
{
    Shop shop;
    GameState oldState;
    [SerializeField] private GameObject UI;
    private void Awake()
    {

        shop = FindFirstObjectByType<Shop>(FindObjectsInactive.Include);
    }

    public void OnInteract()
    {
<<<<<<< HEAD
        if (shop.IsShopOpenUI) 
        {
            print("do nothing");
            shop.CloseShop();
        }
        else 
        {
            print("do something");
            shop.OpenShop();
=======
        if (shop.IsShopOpenUI){

            shop.CloseShop();
            UI.SetActive(true);
            oldState = GameStateScript.CurrentState;
            GameStateScript.CurrentState = GameState.SHOPPING;

        } else{

            shop.OpenShop();
            GameStateScript.CurrentState = oldState;
            UI.SetActive(false);

>>>>>>> cd3827c058b2b532fa526026745bf9a7397ee997
        }
    }
}