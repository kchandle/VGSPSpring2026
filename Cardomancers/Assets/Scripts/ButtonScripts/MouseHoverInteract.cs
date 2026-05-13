using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MouseHoverInteract : MonoBehaviour
{
    public Animator anim;

    void Start()
    {
          anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {   
        GameObject element = GetUIElementUnderMouse();

         if((element != gameObject))
        {   
            anim.enabled = false;
            Debug.Log("Broken");
        }
        else
        {
            Debug.Log("Hovering over: " + element.name);
            anim.enabled = true;
        }
    }

    public static GameObject GetUIElementUnderMouse()
    {
        // Create a pointer event data for the current mouse position
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };
        // Raycast against all UI elements
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);
        // Return the first hit UI element, if any
        return results.Count > 0 ? results[0].gameObject : null;
    }
}
