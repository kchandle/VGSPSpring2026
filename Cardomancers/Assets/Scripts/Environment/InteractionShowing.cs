using UnityEngine;

public class InteractionShowing : MonoBehaviour
{
    // these are the vars
    bool hasCollided = false;
    [SerializeField] string labelText = "Press E to interact";

void OnGUI()
    {
        //if it is in radius it makes a gui telling whats on the string
        if (hasCollided == true)
        {
            GUI.Box(new Rect(140, Screen.height - 50, Screen.width - 300, 120), (labelText));
        }
    }
    //so player is in the radius
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.tag == "Player")

        {
            //sets the vars and label text
            hasCollided = true;

        }
    }
    //whenever player leaves it dissapears
    void OnTriggerExit(Collider other)
    {
        hasCollided = false;

    }
}
