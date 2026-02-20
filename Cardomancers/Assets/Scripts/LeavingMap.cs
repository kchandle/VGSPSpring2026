using UnityEngine;

public class LeavingMap : MonoBehaviour
{

    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
           SaveSystem.Load(player.GetComponent<Inventory>().InventorySO, player);
            Debug.Log(player.gameObject.transform.position);

            Debug.Log("yerterplayer");

        }
        else
        {
            Debug.Log("not player");
        }
    }
}
