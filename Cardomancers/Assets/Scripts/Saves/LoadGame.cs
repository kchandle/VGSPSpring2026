using UnityEngine;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SaveSystem.Load(player.GetComponent<Inventory>().InventorySO, player);
    }
}
