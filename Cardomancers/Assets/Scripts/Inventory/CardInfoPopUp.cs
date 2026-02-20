using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardInfoPopUp : PlayItem
{
    // name of the card
    [SerializeField] private TextMeshProUGUI name; 
    // the description of the card
    [SerializeField] private TextMeshProUGUI description;
    // the type of the card (ice, fire, etc.)
    [SerializeField] private TextMeshProUGUI cardType; 
    // how the card delivers the damage. i.e. damage over time
    [SerializeField] private TextMeshProUGUI damageType; 
    // a silly tagline for the card
    [SerializeField] private TextMeshProUGUI tagLine;
    // the image that goes with the card type
    [SerializeField] private Image typeImage;
    // the image associated with the damage type
    [SerializeField] private Image damageImage;
    // the background
    [SerializeField] private Image backgroundImage;

    [Tooltip("Distance from the center of the card the popup is giving info about")]
    [SerializeField] private float padding;

    
    
    protected override void Update()
    {
        position = GetPopUpLocation();
        base.Update();
    }

    private void SetDescriptions(Card card)
    {
        name.text = card.CardSO.displayName;
        description.text = card.CardSO.description;
        cardType.text = card.CardSO.type;
        //damageType.text = 
        tagLine.text = card.CardSO.tagLine;
    }

    private void OpenPopup()
    {
        name.gameObject.SetActive(true);
        description.gameObject.SetActive(true);
        cardType.gameObject.SetActive(true);
        damageType.gameObject.SetActive(true);
        tagLine.gameObject.SetActive(true);
        typeImage.gameObject.SetActive(true);
        damageImage.gameObject.SetActive(true);
        backgroundImage.gameObject.SetActive(true);
    }

    private void ClosePopup()
    {
        name.gameObject.SetActive(false);
        description.gameObject.SetActive(false);
        cardType.gameObject.SetActive(false);
        damageType.gameObject.SetActive(false);
        tagLine.gameObject.SetActive(false);
        typeImage.gameObject.SetActive(false);
        damageImage.gameObject.SetActive(false);
        backgroundImage.gameObject.SetActive(false);
    }

    private Vector3 GetPopUpLocation()
    {
        Vector3 basePosition = Vector3.zero;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        
        bool focusTargetExists = false;

        if (CardDragInput.focusTarget)
        {
            basePosition = CardDragInput.focusTarget.transform.position;
        }
        else
        {
            ClosePopup();
            return Vector3.zero;
        }

        Vector3[] potentialPositions = new Vector3[4];
        potentialPositions[0] = basePosition + Vector3.up * padding;
        potentialPositions[1] = basePosition + Vector3.right * padding;
        potentialPositions[2] = basePosition + Vector3.down * padding;
        potentialPositions[3] = basePosition + Vector3.left * padding;

        int smallestIndex = 0;
        for (int i = 0; i < 4; i++)
        {
            if (Vector3.Distance(potentialPositions[i], screenCenter) <= Vector3.Distance(potentialPositions[smallestIndex], screenCenter))
            {
                smallestIndex = i;
            }
        }
        
        OpenPopup();
        return potentialPositions[smallestIndex];
    }
}
