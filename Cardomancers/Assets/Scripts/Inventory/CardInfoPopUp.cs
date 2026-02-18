using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private Image backgroundImage;

    [Tooltip("Distance from the center of the card")]
    [SerializeField] private float padding;
}
