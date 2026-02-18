using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardInfoPopUp : PlayItem
{
    // name of the card
    private TextMeshProUGUI name; 
    // the description of the card
    private TextMeshProUGUI description;
    // the type of the card (ice, fire, etc.)
    private TextMeshProUGUI cardType; 
    // how the card delivers the damage. i.e. damage over time
    private TextMeshProUGUI damageType; 
    // a silly tagline for the card
    private TextMeshProUGUI tagLine;
    // the image that goes with the card type
    private Image typeImage;
    // the image associated with the damage type
    private Image damageImage;
    // the background. Should be set in editor
    [SerializeField] private Image backgroundImage;

    [Tooltip("Distance from the center of the card the popup is giving info about")]
    [SerializeField] private float padding;
    
    private Playspace[] playspaces;

    private void Awake()
    {
        playspaces = FindObjectsByType<Playspace>(FindObjectsSortMode.None);
    }

    private void Update()
    {
        position = GetPopUpLocation();
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
        this.gameObject.SetActive(true);
    }

    private void ClosePopup()
    {
        this.gameObject.SetActive(false);
    }

    private Vector3 GetPopUpLocation()
    {
        Vector3 basePosition;
        Vector3 screenCenter = new Vector3(Screen.width / 2f, Screen.height / 2f, 0f);
        
        bool FocusTargetExists = false;

        foreach (Playspace playspace in playspaces)
        {
            if (playspace.focusTarget && playspace.focusTarget is Card)
            {
                FocusTargetExists = true;
                basePosition = playspace.focusTarget.position;
                SetDescriptions((Card)playspace.focusTarget);
            }
        }

        if (!FocusTargetExists)
        {
            ClosePopup();
            return Vector3.zero;
        }

        Vector3[] potentialPositions = new Vector3[4];
        potentialPositions[0] = Vector3.up * padding;
        potentialPositions[1] = Vector3.right * padding;
        potentialPositions[2] = Vector3.down * padding;
        potentialPositions[3] = Vector3.left * padding;

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
