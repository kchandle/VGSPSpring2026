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
    // collects the moneys
    private Inventory inventory;

    // money on here for testing put on SO or sum later
    
    void Awake()
    {
        inventory = GameObject.Find("Player").GetComponent<Inventory>();
    }

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
            inventory.Money++;
            Debug.Log ($"money: {inventory.Money}");
            respawn = true;
            OnCollect.Invoke();
        }
    }
    // getMoney called by interaction scripts
}
