using UnityEngine;

public class moneyPickup : MonoBehaviour
{

    public static int money = 0;

    // money on here for testing put on playercontroller later              

    void OnCollisionEnter(Collision collision)
    {
        if (!collision.collider.CompareTag("Player")) return;
        print (collision);
        money++;
        Debug.Log ($"money: {money}");
    }
    // copied from fps game with unneccesary parts removed, will have to update to use interaction script once micah does that
}
