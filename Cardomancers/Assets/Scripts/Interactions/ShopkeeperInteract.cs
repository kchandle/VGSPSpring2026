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
            UI.SetActive(true);
            oldState = GameStateScript.CurrentState;
            GameStateScript.CurrentState = GameState.SHOPPING;

        } else{

            shop.OpenShop();
            GameStateScript.CurrentState = oldState;
            UI.SetActive(false);

        }
    }
}