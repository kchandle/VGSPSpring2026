using UnityEngine;

public class LoadGame : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        SaveSystem.Load(GameObject.FindGameObjectWithTag("PlayerInventory").GetComponent<Inventory>(), player);
    }
}
