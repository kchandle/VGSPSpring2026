using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerSetup : MonoBehaviour
{
    private BoxCollider trigger;
    
    private void Awake()
    {
        trigger = GetComponent<BoxCollider>();
        trigger.isTrigger = true;
        
        Transform shopTransform = GameObject.FindGameObjectWithTag("StoreLocation").transform;
        this.transform.position = shopTransform.position;
    }
    
    
}
