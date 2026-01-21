using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InteractableObject_SO", menuName = "Scriptable Objects/InteractableObject_SO")]
public class InteractableObject_SO : ScriptableObject
{
    public new string name;
    public UnityEvent interactable = new UnityEvent();
}
