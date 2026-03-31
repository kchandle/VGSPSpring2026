using UnityEngine;

public class DeloaderCollider : MonoBehaviour
{
    public GameObject regionRef;
    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            transform.parent.GetComponent<Deloader>().RegionToggleActive(regionRef);
        }
    }
}
