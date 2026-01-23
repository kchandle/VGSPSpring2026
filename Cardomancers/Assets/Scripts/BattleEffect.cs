using UnityEngine;

public class BattleEffect : MonoBehaviour 
{
    // The amount of damage/heal/stun/whatever the BattleEffect inflicts on the target
    public int StatusAmount;
    //The damage type of the BattleEffect (used for determining weakness/resistance in enemies/player)
//    public DamageType damageType;  NOT IMPLEMENTED YET
    //A list of particle effects to happen when the BattleEffect is played
    ParticleSystem[] particles;
 
    public void PlayParticles(Vector3 pos)
    {
        foreach (ParticleSystem particle in particles)
        {
            Instantiate(particle, pos, Quaternion.identity);
        }
    }

    //The function that is called when the card is played
    public void TriggerEffect()
    {
        //NOT IMPLEMENTED dont have other dependent scripts
    }
}