using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Events;
using System;
using System.Collections;
using UnityEditor;

public class CardDragInput : MonoBehaviour
{

    public Inventory inventory; //set in editor
    private InputActionMap actionMap; // current action map.
    public InputActionAsset inputActions;

    public InventoryUIHandler uIHandler;

    private bool dragDropActive; // if the drag and drop ability is active

    public bool DragDropActive // getter setter for activating dragDropAbility
    {
        get {return dragDropActive;}
        set
        {
            dragDropActive = value;

            if (value == true)
            {
                StartCoroutine(DragDrop());
                
            } else if (value == false)
            {
                StopCoroutine(DragDrop());
            }
        }
    }

    PlayItem dragTarget; // the current PlayItem being dragged (if any)
    Playspace dragPlayspace; // the Playspace that dragTarget is in

    public Vector3 dragTargetStartPos; // starting position of the dragTarget
    public bool isDragging = false; // if a playItem being dragged

    private List<Playspace> activePlayspaces = new List<Playspace>(); // Playspaces that are currently active on the screen

    public event Action<PlayItem, Playspace, Playspace> PlayitemMoved; // PlayItem being moved, To, From


// TEST STUFF FOR TESTING

    public GameObject testPlayItemPrefab;

    public Card_SO testSO;
    public Card_SO testSO2;
      

    // TESTING DONE

    // Used to change the actionMap, as this script is used for the "Battle" ActionMap and "Inventory" ActionMap
    void OnEnable()
    {
        GameStateScript.OnGameStateChanged += ChangeActionMap;
    }
    void OnDisable()
    {
        GameStateScript.OnGameStateChanged -= ChangeActionMap;
    }

    void Start()
    {
       ////  TEST STUFF BEGIN - WILL BE REMOVED LATER
       /// 
        
    //    GameStateScript.UpdateGameState(GameStateScript.GameState.MENU);
    //    AddActivePlayspace(uIHandler.invPlayspace);
    //    AddActivePlayspace(uIHandler.deckPlayspace);
     

    //    uIHandler.DisplayUI();

       //// TEST END
       //DragDropActive = true;



       StartCoroutine(DragDrop());

    }
    
    public void ChangeActionMap(GameStateScript.GameState gameState)
    {   
        InputActionMap newActionMap = null;
        switch (gameState)
        {
            case GameStateScript.GameState.BATTLE: newActionMap = inputActions.FindActionMap("Battle"); break;
            case GameStateScript.GameState.MENU: newActionMap = inputActions.FindActionMap("Inventory"); break;
        }

        if (newActionMap != null)
        {
            actionMap = newActionMap;
        }

    }

// Coroutine for Dragging and Dropping items. When active, the player will be able to drag and drop playItems between the current active playspaces
    public IEnumerator DragDrop()
    {
        print("DragDrop coroutine started");
        dragDropActive = true;
        while (dragDropActive == true)
        {
            
        
            // get player mousePosition;
            Vector3 mousePosition = Pointer.current.position.ReadValue();
            
            



            // get the current focusTarget (Playitem closests to the mouse INSIDE the playspace they are hovering over)
            PlayItem focusTarget;
            if (isDragging == false)
            {
                foreach (Playspace p in activePlayspaces)
                {
                    // try to get focusTarget in Playspace p

                    //Vector3 mousePositionWorld = MouseToWorldWithDistance(mousePosition, p.gameObject);

                    focusTarget = p.GetNearestPlayItem(mousePosition);

                    //if(p.InPlayArea(mousePositionWorld) == true) print(p.name+" is being hovered over");
    
                    
                    p.focusTarget = focusTarget;
                    // For there to be a valid focusTarget, the player must be hovering over p and p must contain Playitems
                    // if there is a valid focusTarget in Playspace p, stop looking for a focusTarget and set dragplaySpace to p
                    if (focusTarget != null)
                    {
                        dragPlayspace = p;
                        
                        break;
                    } else // if no valid focusTarget, check the new active Playspace 
                    {
                        continue;
                    }
                }
            }

            // If the player is currently dragging a PlayItem, move that playItem towards the mousePosition
            if (Pointer.current.press.IsPressed() && isDragging == true && dragPlayspace)
            {
                dragPlayspace.SetDragTarget(dragTarget, mousePosition);
            }

            if (Pointer.current.press.wasPressedThisFrame)
            {
                print("you clicked");

                if (isDragging == false && dragPlayspace != null)
                {
                    
                    // If the player is clicks over an activePlayspace with a valid focusTarget, start dragging that focusTarget

                   
                    dragTarget = dragPlayspace.GetNearestPlayItem(mousePosition);
                    if (dragTarget)
                    {
                        if ((Card)dragTarget)
                        {
                        ((Card)dragTarget).cardImage.gameObject.GetComponent<Canvas>().overrideSorting = true; // override sorting so the card doesn't dissapear when dragged outside of a scrollable playspace
                        ((Card)dragTarget).cardImage.gameObject.GetComponent<Canvas>().sortingOrder = 3;

                        }
                        
                        isDragging = true;
                        dragPlayspace.SetDragTarget(dragTarget, mousePosition);
                    }

                }
            }

            if (Pointer.current.press.wasReleasedThisFrame)
            {
                // if the player was dragging a PlayItem, attempt to move it to a new playSpace
                if (dragTarget)
                {
                    // Gets the Playspace component of the dragTargets parent Playspace

                    GameObject dragTargetParent = dragTarget.gameObject.transform.parent.gameObject;
                    // checking to see which playSpace, if any, the player released the drag target into
                    foreach (Playspace p in activePlayspaces)
                    {
                        //float distanceToPlane = Mathf.Abs(p.transform.position.z - Camera.main.transform.position.z);
                        //mousePosition.z = distanceToPlane;
                        //Vector3 mousePositionWorld = MouseToWorldWithDistance(mousePosition, p.gameObject);

                        // Conditions to successfully move a dragtarget to a new playspace
                        // Player must be hovering their mouse over the New Playspace
                        // New Playspace must not be the current parent of the dragTarget
                        if (p.InPlayArea(mousePosition) && dragTargetParent != p.gameObject)
                        {
                            print("allowed to move to new playspace");
                            // move dragTarget from it's parent to Playspace p
                           
                            

                            // If battling, stop the coroutine if the player successfully plays a card
                            // This prevents them from playing more cards than they are allowed
                            if (FindFirstObjectByType<BattleManager>() != null)
                            {
                                if (FindFirstObjectByType<BattleManager>().isBattling)
                                {
                                    print("we battlin");
                                    if(AttemptPlay((Card)dragTarget, p) == true) {
                                        print("stop the dragdrop");
                                        dragPlayspace.DestroyPlayItem(dragTarget);
                                        dragDropActive = false;
                                        }
                                } 
                            } else {
                                ((Card)dragTarget).cardImage.gameObject.GetComponent<Canvas>().overrideSorting = false;
                                MoveToNewPlayspace(dragTarget, p, dragTargetParent.GetComponent<Playspace>());
                            }
                            //yield break;
                        }
                    }

                    
                    if (dragTarget) ((Card)dragTarget).cardImage.gameObject.GetComponent<Canvas>().overrideSorting = false; // revert back to normal sorting when no longer being dragged
                    dragTarget = null;

                }
                // reset drag variables when the player is no longer dragging
                isDragging = false;
                dragPlayspace = null;
            }
        
        yield return null;
        }
       
    }

    //Tries to play card
    public bool AttemptPlay(Card dragTarget, Playspace p)
    {
        print("Attemping to play " + dragTarget.name);
        if (dragTarget != null)
        {
            print("dragtarget has a card component");
            //tries to play card against the playspace's parent gameobject enemy component
            dragTarget.TryPlayCard(p.gameObject.GetComponentInParent<Enemy>());
            return true;
        }
        else{
            return false;
        }

    }

    // Empties the activePlayspaces list  
    public void ClearActivePlayspaces()
    {
        activePlayspaces.Clear();
    }

// Adds a playspace to activePlayspaces if it isn't already in the list
    public void AddActivePlayspace(Playspace playspace)
    {
        if(!activePlayspaces.Contains(playspace)) activePlayspaces.Add(playspace);
    }

// Remove a Playspace from activePlayspaces if it is in the list
    public void RemoveActivePlayspace(Playspace playspace)
    {
        if(activePlayspaces.Contains(playspace)) activePlayspaces.Remove(playspace);
    }


    // moves a PlayItem from one playSpace to another by destroying it and reinstancing it
    public void MoveToNewPlayspace(PlayItem moveTarget, Playspace to, Playspace from)
    {
        // only move the item if the "to" PlaySpace can receive PlayItems from the "from" PlaySpace
        if (to.allowedDonors.Contains(from)){
            print("in allowed donors");
            to.NewPlayItem(moveTarget.gameObject, ((Card)moveTarget).CardSO, ((Card)moveTarget).inventoryCard);
            from.DestroyPlayItem(moveTarget);
            PlayitemMoved.Invoke(moveTarget, to, from);
        }

    }

    // gets correct mousePosition if using a Screen Space - Camera canvas
    // currently not in use
    Vector2 MouseToWorldWithDistance(Vector3 screenPosition, GameObject target)
    {
        float distanceToPlane = Mathf.Abs(target.transform.position.z - Camera.main.transform.position.z);
        screenPosition.z = distanceToPlane;
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }
}
