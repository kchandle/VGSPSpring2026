using UnityEngine;

public class TempSaveTest : MonoBehaviour
{
    [SerializeField] Inventory_SO inventory;

    void Start()
    {
        InventorySaveSystem.Load(inventory);
    }
}
