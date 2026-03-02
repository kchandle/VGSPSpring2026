using System.Linq;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Card_SO testDefaultCard;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Inventory.CardsDatabase = Resources.LoadAll<Card_SO>("Card_SO").ToDictionary(k => k.name, v => v);
        Inventory.HacksDatabase = Resources.LoadAll<Hack_SO>("Hack_SO").ToDictionary(k => k.name, v => v);
        if(Inventory.InventoryList.Count == 0) Inventory.AddCardToInventory(testDefaultCard, 3);
        SaveSystem.Load(player);
        Destroy(this);
    }
}
