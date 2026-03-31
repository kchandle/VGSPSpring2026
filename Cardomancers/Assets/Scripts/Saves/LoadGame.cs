using System.Linq;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Card_SO testDefaultCard;
    [SerializeField] private Hack_SO testDefaultHack;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Inventory.CardsDatabase = Resources.LoadAll<Card_SO>("Card_SO").ToDictionary(k => k.name, v => v);
        Inventory.HacksDatabase = Resources.LoadAll<Hack_SO>("Hack_SO").ToDictionary(k => k.name, v => v);
        SaveSystem.Load(player);
        if (Inventory.InventoryList.Count == 0)
        {
            //Inventory.AddCardToInventory(testDefaultCard, 3);
            //Inventory.AddHackToInventory(testDefaultHack, 1);
            //mr chandlee told me to get rid of this s
        }
        Destroy(this);
    }
}
