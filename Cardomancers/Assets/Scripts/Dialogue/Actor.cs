using UnityEngine;
using TMPro;
using DialogueScripts;

// An interacteable gameobject that causes dialogue to appear on the screen when interacted with
public class Actor : MonoBehaviour
{


    [SerializeField] private DialogueSO dialogue; // the dialogue scene to play when the player interacts with the Actor

    public DialogueSO Dialogue
    {
        get => dialogue; 
        set
        {
            dialogue = value;
        }
    }

    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
}
