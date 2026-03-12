using UnityEngine;
using UnityEngine.Events;

public class CardInteractable : MonoBehaviour
{
    //unity event gets called whenever the card hits the object 
    public UnityEvent OnCardHit = new UnityEvent();
}