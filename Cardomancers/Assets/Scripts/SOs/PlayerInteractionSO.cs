using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerInteractionSO : MonoBehaviour
{
    [SerializeField]InteractableObject_SO inter;

    // public InteractableObject interactableObject;
    public InputActionReference interact;

    void Awake()
    {
        inter = GetComponent<InteractableObject_SO>();
    }

    public void InteractKey(InputAction.CallbackContext obj)
    {
       Debug.Log("Yayy, u inteactedd with mee!");
    }

    // void Update()
    // {

    // }
}

