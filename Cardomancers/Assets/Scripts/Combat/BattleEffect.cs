using UnityEngine;

public enum DamageType
{
    
Ice,
Fire,
Water,
Earth,
Wind,
Light,
Lightning,
Poison,
Dark,
DamageBlock,
Psychic,
None
}

public class BattleEffect : MonoBehaviour 
{
    // The amount of damage/heal/stun/whatever the BattleEffect inflicts on the target
    public int StatusAmount;
    //The damage type of the BattleEffect (used for determining weakness/resistance in enemies/player)
    public DamageType damageType; 
    //A list of particle effects to happen when the BattleEffect is played
    ParticleSystem[] particles;

    public float DamageMult = 2.0f; // Multiplier for damage if weakness is present
    public float DamageReduct = 0.5f; // Multiplier for damage if resistance is present

    public void PlayParticles(Vector3 pos)
    {
        foreach (ParticleSystem particle in particles)
        {
            Instantiate(particle, pos, Quaternion.identity);
        }
    }

    //The function that is called when the card is played
    public void TriggerEffect(GameObject target)
    {
        bool isEnemy = target.CompareTag("Enemy");
        bool isPlayer = target.CompareTag("Player");

        int DamageDealt = StatusAmount;

        if (isEnemy)
        {
            Enemy enemy = target.GetComponent<Enemy>();
            foreach (DamageType resistance in enemy.resistances)
            {
                if (resistance == damageType)
                {
                    DamageDealt = Mathf.RoundToInt(StatusAmount * DamageReduct);
                    break;
                }
            }
            foreach (DamageType weakness in enemy.weaknesses)
            {
                if (weakness == damageType)
                {
                    DamageDealt = Mathf.RoundToInt(StatusAmount * DamageMult);
                    break;
                }
            }
            enemy.currentHealth -= DamageDealt;
        }
        else if (isPlayer)
        {
            target.GetComponent<PlayerController>().currentHealth -= StatusAmount;
        }

        //NOT IMPLEMENTED dont have other dependent scripts
    }
}