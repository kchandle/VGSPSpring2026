using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;


public class CardPickupPrompt : MonoBehaviour
{
    //Upon picking up a card, make this item appear and show for 5 seconds, then disappear again

    //Set in inspector
    [SerializeField] private Image pickupPromptBackground;
    [SerializeField] private TMPro.TextMeshProUGUI pickupDescription;
    [SerializeField] private TMPro.TextMeshProUGUI pickupName;
    [SerializeField] private Image pickupImage;

    //card the prompt is for
    private Card_SO card;

    //Seconds before the prompt will start to fade
    [SerializeField]private float lifespan = 3.0f;
    //Seconds it takes to fade
    [SerializeField]private float fadeDuration = 2.0f;

    private bool dispelPrompt;

    void Awake()
    {
        this.gameObject.SetActive(false);
    }

    //Note, the prompt game object will have to be activset active by the GetCard() event of the card it's picking up 
    public void CreatePrompt(cardPickup pickup)
    {
        dispelPrompt = false;

        //Make sure the UI elements aren't transparent when they appear
        pickupPromptBackground.CrossFadeAlpha(1, 0, true);
        pickupDescription.CrossFadeAlpha(1, 0, true);
        pickupName.CrossFadeAlpha(1, 0, true);
        pickupImage.CrossFadeAlpha(1, 0, true);

        //Set the prompt's visual elements according to the cardPickup's card
        card = pickup.card.cardSO;
        pickupDescription.text = "" + card.description;
        pickupName.text = "New Card!: " + card.displayName;
        pickupImage.sprite = card.image;

        StartCoroutine(Wait(lifespan, fadeDuration));
    }

    //Wait a few seconds, then make the prompt fade over a few seconds before setting it inactive
    IEnumerator Wait(float lifespan, float fadeDuration)
    {
        
        yield return new WaitForSeconds(lifespan);

        pickupPromptBackground.CrossFadeAlpha(0, fadeDuration, true);
        pickupDescription.CrossFadeAlpha(0, fadeDuration, true);
        pickupName.CrossFadeAlpha(0, fadeDuration, true);
        pickupImage.CrossFadeAlpha(0, fadeDuration, true);
        
        dispelPrompt = true;
        yield return new WaitForSeconds(fadeDuration);
        if(dispelPrompt)
        {
            this.gameObject.SetActive(false);
        }
        
    }




    //Just in case we want this popup to appear in other ways in the future
    /*public void CreatePrompt(Card_SO cardSO)
    {
        dispelPrompt = false;

        //Make sure the UI elements aren't transparent when they appear
        pickupPromptBackground.CrossFadeAlpha(1, 0, true);
        pickupDescription.CrossFadeAlpha(1, 0, true);
        pickupName.CrossFadeAlpha(1, 0, true);
        pickupImage.CrossFadeAlpha(1, 0, true);

        card = cardSO;
        pickupDescription.text = "" + card.description;
        pickupName.text = "New Card!: " + card.displayName;
        pickupImage.sprite = card.image;

        StartCoroutine(Wait(lifespan, fadeDuration));
    }*/
    
}
