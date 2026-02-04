using UnityEngine;
using UnityEngine.Events;

public class moneyPickup : MonoBehaviour
{

    public static int money = 0;
    public bool respawn = false;
    public float respawnTime = 15f;
    public UnityEvent Respawn = new UnityEvent();
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
        money++;
        Debug.Log ($"money: {money}");
        respawn = true;
        OnCollect.Invoke();
    }
    // getMoney called by interaction scripts
}
