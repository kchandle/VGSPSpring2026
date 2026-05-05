using UnityEngine;
using UnityEngine.InputSystem;

public class InventoryTutorial : MonoBehaviour
{
    public InputActionReference inventoryAction;

    public void Update()
    {
        if (inventoryAction.action.IsPressed())
        {
            gameObject.SetActive(false);
        }

    }
}