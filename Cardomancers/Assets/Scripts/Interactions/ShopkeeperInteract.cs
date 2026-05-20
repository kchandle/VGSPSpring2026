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
        if (shop.IsShopOpenUI){

            shop.CloseShop();
            UI.SetActive(false);
            GameStateScript.CurrentState = GameState.WALKING;

        } else{

            shop.OpenShop();
            oldState = GameStateScript.CurrentState;
            GameStateScript.CurrentState = GameState.SHOPPING;
            UI.SetActive(true);

        }
    }
}