using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TMPro;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.UIElements;



// some parts of this script are commented out because they were part of this script on a previous project
// they're currently being kept here in case we need them later
namespace DialogueScripts
{
    

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance; // singleton instance
    public Animator animator; // the current animator changing the talksprite


    [SerializeField]
    
    public float textDelay; // delay between characters
    public bool reactive; // If the text reactivates if the player returns to that NPC

    private int index; //Current line being displayed
    [SerializeField] GameObject canvas; // the canvas containing the dialogue GUI

    public GameObject textBoxHolder; // the text box holder
    public GameObject titleBoxHolder; // title box holder.
    public GameObject spriteBorder; // the sprite border.


    public TextMeshProUGUI textElement; // the current text box the dialogue text is being loaded into
    public TextMeshProUGUI titleElement; // the current text box the dialogue speaker is being loaded into
    public UnityEngine.UI.Image talkspriteImage; // the image element where the talksprite will be loaded

    public InputActionAsset inputActions; //The set of actions the player can perform, reference used to react to player input
    public InputAction nextAction;

    public DialogueSO dialogue; // current dialogue SO


    public Transform playerTransform;
    public CinemachineOrbitalFollow camPosition;
    public CinemachineRotationComposer camRotation;
        public PlayerCamera cameraScript; 
        public CinemachineCamera cam;
        public CinemachineInputAxisController input;
        CinemachineBrain brain;
       
        public Transform mainCam;
    // Assign the player's transform in the Inspector
    public StartBattle reference;

    //Gets player action map to react to player input
    private void Awake()
    {  
        // Check if an instance already exists
        if (instance != null && instance != this)
        {
            // If so, destroy this new object to ensure only one instance remains
            Destroy(this.gameObject);
            return;
        }
        // Otherwise, set the instance to this object
        instance = this;

        playerTransform = GameObject.FindWithTag("Player").transform;
        nextAction = inputActions.FindActionMap("MapWalking").FindAction("Interact");

        brain = FindFirstObjectByType<CinemachineBrain>();
            input = FindFirstObjectByType<CinemachineInputAxisController>();
            cameraScript = FindFirstObjectByType<PlayerCamera>();
           
        mainCam = Camera.main.transform;


        // Optional: Keep the object alive when loading new scenes
        DontDestroyOnLoad(this.gameObject);
    }

    public void AssignStartBattle(StartBattle starter)
    {
        print("assigned " + starter.name);
        reference = starter;
        print(reference.name);
    }

        //void Start()
        //{
        //    StartDialogue(dialogue);
        //}

        // Update is called once per frame

        Coroutine cor;
    void Update()
    {
        if(canvas.activeInHierarchy)
        {
            if (nextAction.WasPressedThisFrame())
            {
                //Checks if line is finished typing and either skips to next line or finishes current line on player input
                if (textElement.text == dialogue.lines[index].text)
                {
                    NextLine();
                }
                else
                {
                    StopCoroutine(cor);
                    textElement.text = dialogue.lines[index].text;
                }
            }
        }
    }


    // Starts a Dialogue scene based 
    public void StartDialogue(DialogueSO newDialogue)
    {
        index = 0;

        // textBoxHolder.GetComponent<Image>().color = newDialogue.textBackgroundColorDefault;
        // titleBoxHolder.GetComponent<Image>().color = newDialogue.titleBackgroundColorDefault;
        // spriteBorder.GetComponent<Image>().color = newDialogue.spriteColorDefault;

        textElement.color = newDialogue.textColorDefault;
        titleElement.color = newDialogue.titleTextColorDefault;

        dialogue = newDialogue;
        canvas.SetActive(true);

        DialogueEvents.StartDialogue(newDialogue);

        GameStateScript.CurrentState = GameStateScript.GameState.SPEAKING;

            
        brain.enabled = false;
            input.enabled = false;
            cameraScript.enabled = false;
        cor = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
            if (dialogue.lines[index].lineHasCutscene)
            {
                CharacterController cc = playerTransform.GetComponent<CharacterController>();
                cc.enabled = false;
                playerTransform.position = dialogue.lines[index].playerMovePosition;
                playerTransform.eulerAngles = dialogue.lines[index].playerMoveRotation;
                cc.enabled = true;

                if (camCor != null) StopCoroutine(camCor);
                camCor = StartCoroutine(LerpCam(dialogue.lines[index].cameraMovePosition, dialogue.lines[index].cameraRotation));

            }

            textElement.text = string.Empty;
        titleElement.text = dialogue.lines[index].displayName;
        talkspriteImage.sprite = dialogue.lines[index].talksprite;

        foreach (char c in dialogue.lines[index].text.ToCharArray())
        {
            textElement.text += c;
            yield return new WaitForSeconds(dialogue.lines[index].textDelay);
        }
    }

        private Coroutine camCor;
    void NextLine( )
    {
        if (index < dialogue.lines.Length - 1)
        {
            index++;
            
            textElement.text = string.Empty;

            StartCoroutine(TypeLine());            
        }
        else
        {
            
            textElement.text = string.Empty;
            canvas.SetActive(false);
            if(dialogue.startsBattle)
            {
                reference.StartBattleNow();
            }
            else
            {
                brain.enabled = true;
                    input.enabled = true;
                    cameraScript.enabled = true;
                    playerTransform.gameObject.GetComponent<PlayerInteract>().interacting = false;
                GameStateScript.CurrentState = GameStateScript.GameState.WALKING;
            }
            //if (!reactive)
            //{
            //    //gameObject.SetActive(false);
            //}
        }

    }

    public IEnumerator LerpCam(Vector3 newCamPosition, Vector3 newCamRotation)
    {
            Camera mainCam = Camera.main;
            mainCam.transform.position = newCamPosition;
            
            Quaternion newRot = Quaternion.Euler(newCamRotation);
            while (Quaternion.Angle(newRot, mainCam.transform.rotation) > 0.01)
            {
                mainCam.transform.rotation = Quaternion.Slerp(mainCam.transform.rotation, newRot, Time.deltaTime * 3f);
                yield return null;
            }

            mainCam.transform.eulerAngles = newCamRotation;
            camCor = null;
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
