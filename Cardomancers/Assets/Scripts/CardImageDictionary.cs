using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardImageDictionary : MonoBehaviour
{
    #region Image refs
    public Image imageNone;
    public Image imageIce;
    public Image imageFire;
    public Image imageWater;
    public Image imageWind;
    public Image imageLight;
    public Image imageLightning;
    public Image imagePoison;
    public Image imageDark;
    public Image imageDamageBlock;
    public Image imagePsychic;
    public Image imageStun;
    // Image for each damage type
    #endregion

    public Dictionary<DamageType, Image> cardImageDictionary = new Dictionary<DamageType, Image>();
    public void Awake()
    {
        
        cardImageDictionary.Add(DamageType.None, imageNone);
        cardImageDictionary.Add(DamageType.Ice, imageIce);
        cardImageDictionary.Add(DamageType.Fire, imageFire);
        cardImageDictionary.Add(DamageType.Water, imageWater);
        cardImageDictionary.Add(DamageType.Wind, imageWind);
        cardImageDictionary.Add(DamageType.Light, imageLight);
        cardImageDictionary.Add(DamageType.Lightning, imageLightning);
        cardImageDictionary.Add(DamageType.Poison, imagePoison);
        cardImageDictionary.Add(DamageType.Dark, imageDark);
        cardImageDictionary.Add(DamageType.DamageBlock, imageDamageBlock);
        cardImageDictionary.Add(DamageType.Psychic, imagePsychic);
        cardImageDictionary.Add(DamageType.Stun, imageStun);
        
    }
}
