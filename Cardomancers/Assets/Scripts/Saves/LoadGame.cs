using System.Linq;
using UnityEngine;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        Inventory.CardsDatabase = Resources.LoadAll<Card_SO>("Card_SO").ToDictionary(k => k.name, v => v);
        Inventory.HacksDatabase = Resources.LoadAll<Hack_SO>("Hack_SO").ToDictionary(k => k.name, v => v);
        SaveSystem.Load(player);
        Destroy(this);
    }
}
