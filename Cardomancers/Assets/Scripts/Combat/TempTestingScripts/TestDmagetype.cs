using UnityEngine;

public class TestDmagetype : MonoBehaviour
{
    public Card_SO card;

     void Start()
    {
        foreach (BattleEffect e in card.cardEffects)
        {
            print(e.damageType);
        }
    }
}
