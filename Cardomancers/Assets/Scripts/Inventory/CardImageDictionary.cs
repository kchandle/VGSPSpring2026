using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class CardImageDictionary : MonoBehaviour
{
    #region Image refs
    public Sprite imageNone;
    public Sprite imageIce;
    public Sprite imageFire;
    public Sprite imageWater;
    public Sprite imageWind;
    public Sprite imageLight;
    public Sprite imageLightning;
    public Sprite imagePoison;
    public Sprite imageDark;
    public Sprite imageDamageBlock;
    public Sprite imagePsychic;
    public Sprite imageStun;
    // Image for each damage type
    public Sprite imageDamageInst;
    public Sprite imageHealInst;



    #endregion

    public static Dictionary<DamageType, Sprite> cardElementImageDictionary = new Dictionary<DamageType, Sprite>();
    public static Dictionary<damageType, Sprite> cardAttackTypeDictionary = new Dictionary<damageType, Sprite>();
    public void Awake()
    {

        // Additions to the cardElementImageDictionary dict
        cardElementImageDictionary.Add(DamageType.None, imageNone);
        cardElementImageDictionary.Add(DamageType.Ice, imageIce);
        cardElementImageDictionary.Add(DamageType.Fire, imageFire);
        cardElementImageDictionary.Add(DamageType.Water, imageWater);
        cardElementImageDictionary.Add(DamageType.Wind, imageWind);
        cardElementImageDictionary.Add(DamageType.Light, imageLight);
        cardElementImageDictionary.Add(DamageType.Lightning, imageLightning);
        cardElementImageDictionary.Add(DamageType.Poison, imagePoison);
        cardElementImageDictionary.Add(DamageType.Dark, imageDark);
        cardElementImageDictionary.Add(DamageType.DamageBlock, imageDamageBlock);
        cardElementImageDictionary.Add(DamageType.Psychic, imagePsychic);
        //cardElementImageDictionary.Add(DamageType.Stun, imageStun);

        // Additions to the cardAttackTypeDictionary dict
        cardAttackTypeDictionary.Add(damageType.damageInstant, imageDamageInst);
        cardAttackTypeDictionary.Add(damageType.healInstant, imageHealInst);
    }
}
