using UnityEngine;
using TMPro;
using System.Collections;
using Unity.VisualScripting;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.InputSystem;
using UnityEngine.UI;


// some parts of this script are commented out because they were part of this script on a previous project
// they're currently being kept here in case we need them later
namespace DialogueScripts
{
    

public class DialogueManager : MonoBehaviour
{

    public Animator animator; // the current animator changing the talksprite


    [SerializeField]
    
    public float textDelay; // delay between characters
    public bool reactive; // If the text reactivates if the player returns to that NPC

    private int index; //Current line being displayed
    [SerializeField] GameObject canvas; // the canvas containing the dialogue GUI

    public TextMeshProUGUI textElement; // the current text box the dialogue is being loaded into
    public TextMeshProUGUI titleElement; // the current text box the dialogue is being loaded into
    public Image talkspriteImage; // the image element where the talksprite will be loaded

    public InputActionAsset inputActions;
    InputAction nextAction;

    public DialogueSO dialogue; // current dialogue SO


    public Transform playerTransform; // Assign the player's transform in the Inspector


    private void Awake()
    { 
        nextAction = inputActions.FindActionMap("MapWalking").FindAction("Next");  
    }

    void Start()
    {
        //StartDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (nextAction.WasPressedThisFrame())
        {
            
            if (textElement.text == dialogue.lines[index].text)
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textElement.text = dialogue.lines[index].text;
            }
        }
    }

    // private void OnTriggerEnter2D(Collider2D collision)
    // {
    //     canvas.SetActive(true);
    //     StartDialogue();
    // }

    // Starts a Dialogue scene based 
    public void StartDialogue(DialogueSO newDialogue)
    {
        index = 0;

        dialogue = newDialogue;
        canvas.SetActive(true);


        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        textElement.text = string.Empty;
        titleElement.text = dialogue.lines[index].displayName;
        talkspriteImage.sprite = dialogue.lines[index].talksprite;
        foreach (char c in dialogue.lines[index].text.ToCharArray())
        {
            textElement.text += c;
            yield return new WaitForSeconds(dialogue.lines[index].textDelay);
        }
    }
    void NextLine( )
    {
        if (index < dialogue.lines.Length - 1)
        {
            index++;
            

            StartCoroutine(TypeLine());
            textElement.text = string.Empty;
        }
        else
        {
            textElement.text = string.Empty;
            canvas.SetActive(false);
            if (!reactive)
            {
                //gameObject.SetActive(false);
            }
        }

    }

    // Get closest object to player.
        public GameObject GetClosestObject(List<GameObject> objectsToSearch, Transform player)
    {
        if (objectsToSearch == null || objectsToSearch.Count == 0 || player == null)
        {
            return null;
        }

        GameObject closestObject = null;
        float minDistance = Mathf.Infinity;
        Vector3 playerPosition = player.position;

        foreach (GameObject obj in objectsToSearch)
        {
            if (obj == null) continue; // Skip null entries in the list

            float distance = Vector3.Distance(obj.transform.position, playerPosition);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestObject = obj;
            }
        }
        return closestObject;
    }
}

}
