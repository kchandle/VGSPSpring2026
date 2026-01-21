using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    // List<GameObject> hits;



    [SerializeField] InteractableObject inter;
    public InputActionReference interact;

  

    CapsuleCollider capsule;
    public int range = 5;

    void Awake()
    {
        capsule = GetComponent<CapsuleCollider>();
        inter = GetComponent<InteractableObject>();
        // hits = new List<GameObject>();
    }



    public void InteractKey(InputAction.CallbackContext obj)
    {
       Debug.Log("Yayy, u inteactedd with mee!");
    }



    void FixedUpdate()
    {
        RaycastHit hit;

        //Radius in which objects can be interacted with
        Collider[] col = Physics.OverlapSphere(transform.position, range);
        {
            //If object is interactable, do stuff
            print(col);





            /*if(hit.transform.gameObject.TryGetComponent<>(out  interactable))
            {
                
            }*/
        }

    }

}
