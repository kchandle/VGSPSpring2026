using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static GameStateScript; 

public class PlayerInteract : MonoBehaviour
{
    public bool interacting = false;
    // the range of the area player can interact with things in:
    public int range = 5;

    public bool InRange { get; private set; } = false;

    public InteractableObject currentHighlight = null;
    public GameObject interactPrompt;
    public GameStateScript.GameState state;
    public BattleManager battleManager;
    public bool battling;

    public void Start()
    {
        GameStateScript.GameState state = GameStateScript.CurrentState;
    }

    private void Update()
    {
        InteractHighlight();

        if (GameStateScript.CurrentState == GameState.WALKING) interactPrompt.SetActive(InRange);
        else interactPrompt.SetActive(false); 
    }




    //if the interactkey is set to being interacted or whatever, basically if u press the key:
    public void OnInteract(InputAction.CallbackContext obj)
    {
        if (!obj.started) return;
        if(interacting) return;
        // Checking CurrentState to make sure you can't interact while in battle
        if (GameStateScript.CurrentState == GameStateScript.GameState.INVENTORY) return;
        if (GameStateScript.CurrentState == GameStateScript.GameState.BATTLE) return;
        if (GameStateScript.CurrentState == GameStateScript.GameState.SPEAKING) return;

        // sends an array thing to get all objects:
        Collider[] col = Physics.OverlapSphere(transform.position, range);
        {
            float minRange = 1000f;
            //If object is interactable, so basically if it has the interactable object script, do what it needs to do:
            InteractableObject interactable = null;
            foreach (Collider c in col)
            {
                if (c.TryGetComponent(out InteractableObject inter))
                {
                    float range = (inter.transform.position - transform.position).magnitude;
                    if (range < minRange)
                    {
                        interactable = inter;
                        minRange = range;
                    }
                }

                if (interactable != null)
                {
                    interacting = true;
                    interactable.interactable.Invoke();
                }
            }
        }
    }

    public void InteractHighlight()
    {
        Collider[] col = Physics.OverlapSphere(transform.position, range);
        {
            float minRange = 1000f;
            //If object is interactable, so basically if it has the interactable object script, do what it needs to do:
            InteractableObject interactable = null;
            InRange = false;
            foreach (Collider c in col)
            {
                if (c.TryGetComponent(out InteractableObject inter))
                {
                    float range = (inter.transform.position - transform.position).magnitude;
                    if (range < minRange)
                    {
                        interactable = inter;
                        minRange = range;
                    }
                    if (currentHighlight != null)
                    {
                        ChangeAllChildrenLayer(currentHighlight.gameObject, "Default");
                        if (currentHighlight.highlightables.Length > 0) foreach (GameObject g in currentHighlight.highlightables) ChangeAllChildrenLayer(g, "Default");
                    }

                    ChangeAllChildrenLayer(interactable.gameObject, "Outline");
                    if (inter.highlightables.Length > 0) foreach(GameObject g in inter.highlightables) ChangeAllChildrenLayer(g, "Outline");
                    currentHighlight = interactable;
                    InRange = true;
                }
                else if (currentHighlight != null && !InRange)
                {
                    ChangeAllChildrenLayer(currentHighlight.gameObject, "Default");
                    if (currentHighlight.highlightables.Length > 0) foreach (GameObject g in currentHighlight.highlightables) ChangeAllChildrenLayer(g, "Default");
                    currentHighlight = null;
                }
            }

        }
    }

    public void AUDBIEfwk()
    {
        interacting = false;
    }

    public void ChangeAllChildrenLayer(GameObject target, string layer)
    {
        target.gameObject.layer = LayerMask.NameToLayer(layer);
        foreach (Transform child in target.transform)
        {
            bool ignoreTag = child.CompareTag("IgnoreHighlight");
            if (ignoreTag && child.childCount <= 0) continue;
            else if (ignoreTag)
            {
                ChangeAllChildrenLayer(child.gameObject, layer);
                continue;
            }
            child.gameObject.layer = LayerMask.NameToLayer(layer);
            if (child.childCount > 0) ChangeAllChildrenLayer(child.gameObject, layer);
        }
    }

}