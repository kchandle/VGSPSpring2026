using UnityEngine;
using UnityEngine.UIElements;

[RequireComponent(typeof(BoxCollider))]
public class TriggerSetupConius : MonoBehaviour
{
    BoxCollider boxTrigger;

    private void Awake()
    {
        boxTrigger = GetComponent<BoxCollider>();
        boxTrigger.isTrigger = true;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //TODO: Create ConiusApartment tag and apply it to the model for conius' apartment
        transform.SetParent(GameObject.FindGameObjectWithTag("ConiusApartment").transform);
        transform.localPosition = Vector3.zero;
    }
}
