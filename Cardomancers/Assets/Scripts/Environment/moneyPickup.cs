using UnityEngine;
using UnityEngine.Events;

public class moneyPickup : MonoBehaviour
{
    public bool respawn = false;
    // respawn bool causes respawn countdown
    public float respawnTime = 15f;
    // time until respawn occurs
    public UnityEvent Respawn = new UnityEvent();
    // respawns the moneys
    public UnityEvent OnCollect = new UnityEvent();

    // money on here for testing put on SO or sum later
    
    public void Update()
    {
        if (respawn)
        {
            respawnTime -= Time.deltaTime;
            if (respawnTime < 0)
            {
                Respawn.Invoke();
                respawn = false;
                respawnTime = 15;
            }
        }
    }
    // i dont remember how unity events actually work bruh

    public void getMoney()
    {
        if(!respawn)
        {
            Inventory.Money++;
            Debug.Log ($"money: {Inventory.Money}");
            respawn = true;
            OnCollect.Invoke();
        }
    }
    // getMoney called by interaction scripts
}
