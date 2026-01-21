using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu(menuName = "Interaction Objects")]
public class PlayerInteractionSO : ScriptableObject
{
    // public InteractableObject interactableObject;
     public InputActionReference interact;

    void InteractKey(InputAction.CallbackContext obj)
    {
       Debug.Log("Yayy, u inteactedd with mee!");
    }

    // void Update()
    // {

    // }
}

