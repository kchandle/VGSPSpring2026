using UnityEngine;
using System.Collections.Generic;

public class GetHacksOnEvent : MonoBehaviour
{
    public List<Hack_SO> hacksToGet;
    public GameObject player;

    public void Awake()
    {
        player = GameObject.FindWithTag("Player");
    }

    public void GiveHacks()
    {

        foreach (Hack_SO hack in hacksToGet)
        {
            Inventory.AddHackToInventory(hack);
        }

        hacksToGet.Clear();
        player.GetComponent<PlayerInteract>().interacting = false;
    }
}
