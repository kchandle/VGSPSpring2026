using UnityEngine;

public class DeloaderCollider : MonoBehaviour
{
    public GameObject regionRef;

    public void Awake()
    {
        regionRef.SetActive(false);
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.GetComponent<Deloader>().RegionToggleActive(regionRef);
        }
    }
    
    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            transform.parent.GetComponent<Deloader>().RegionToggleActive(regionRef);
        }
    }
}
