using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    public new string name;
    public UnityEvent<GameObject> interactable;
}
