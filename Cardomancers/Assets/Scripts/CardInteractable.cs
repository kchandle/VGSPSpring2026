using UnityEngine;
using UnityEngine.Events;

public class CardInteractable : MonoBehaviour
{
    public UnityEvent OnCardHit = new UnityEvent(); //unity event gets called whenever the card hits the object 
}