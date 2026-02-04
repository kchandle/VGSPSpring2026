using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.Events;
using System.Collections;
using UnityEditor;

public class CardDragInput : MonoBehaviour
{

    private InputActionMap actionMap; // current action map.



    public InputActionAsset inputActions;

    private bool dragDropActive; // if the drag and drop ability is active

    public bool DragDropActive
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

    public Vector3 dragTargetStartPos;
    public bool isDragging = false;

    private List<Playspace> activePlayspaces = new List<Playspace>(); // Playspaces that are currently active on the screen


// TEST STUFF FOR TESTING
    public Playspace testPlayspace1;

    public Playspace testPlayspace2;

    public Playspace testPlayspace3;

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

    //void Start()
    //{
    //    ////  TEST STUFF BEGIN - WILL BE REMOVED LATER
    //    /// 
        
    //    GameStateScript.UpdateGameState(GameStateScript.GameState.MENU);
    //    AddActivePlayspace(testPlayspace1);
    //    AddActivePlayspace(testPlayspace2);
    //    AddActivePlayspace(testPlayspace3);

    //    testPlayspace1.NewPlayItem(testPlayItemPrefab, testSO);
    //    testPlayspace1.NewPlayItem(testPlayItemPrefab, testSO);
    //    testPlayspace1.NewPlayItem(testPlayItemPrefab, testSO);
    //    testPlayspace1.NewPlayItem(testPlayItemPrefab, testSO2);
    //    testPlayspace1.NewPlayItem(testPlayItemPrefab, testSO2);
    //    testPlayspace2.NewPlayItem(testPlayItemPrefab, testSO);
    //    testPlayspace2.NewPlayItem(testPlayItemPrefab, testSO2);
    //    testPlayspace3.NewPlayItem(testPlayItemPrefab, testSO2);
    //    //// TEST END
    //    DragDropActive = true;

    //    //StartCoroutine(DragDrop());

    //}
    
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

    //public IEnumerator TestCo()
    //{
    //    print ("test co started");
    //    yield return null;
    //}
    public IEnumerator DragDrop()
    {
        dragDropActive = true;
        while (dragDropActive == true)
        {
            
        
            // get player mousePosition;
            Vector3 mousePosition = Pointer.current.position.ReadValue();
            
            

            //mousePosition.z = 0f;
            //print(mousePosition);

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


            if (Pointer.current.press.IsPressed() && isDragging == true && dragPlayspace)
            {
                //Vector3 mousePositionWorldDragPlayspace = MouseToWorldWithDistance(mousePosition, dragPlayspace.gameObject);
                dragPlayspace.SetDragTarget(dragTarget, mousePosition);
            }

            if (Pointer.current.press.wasPressedThisFrame)
            {

                if (isDragging == false && dragPlayspace != null)
                {
                    
                    // get a DragTarget
                    //Vector3 mousePositionWorldDragPlayspace = MouseToWorldWithDistance(mousePosition, dragPlayspace.gameObject);
                    dragTarget = dragPlayspace.GetNearestPlayItem(mousePosition);
                    isDragging = true;
                    dragPlayspace.SetDragTarget(dragTarget, mousePosition);
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
                            // move dragTarget from it's parent to Playspace p
                           
                            

                            // make it so this is only true if in battle mode
                            if (Object.FindFirstObjectByType<BattleManager>() != null)
                            {
                                BattleManager bm = Object.FindFirstObjectByType<BattleManager>();
                                if (Object.FindFirstObjectByType<BattleManager>().isBattling)
                                {
                                    if(AttemptPlay((Card)dragTarget, p) == true) 
                                    {
                                        if (bm.PlayerDeckCopy.Contains(dragTarget.GetComponent<Card>().inventoryCard))
                                        {
                                            bm.PlayerDeckCopy.Remove(dragTarget.GetComponent<Card>().inventoryCard);
                                        }
                                        dragPlayspace.DestroyPlayItem(dragTarget);
                                        dragDropActive = false;
                                    }
                                } else
                                {
                                    MoveToNewPlayspace(dragTarget, p, dragTargetParent.GetComponent<Playspace>());
                                }
                            }
                            //yield break;
                        }
                    }

                    //add a hack to a card if the dragTarget is an inventory hack
                    if (dragTarget is InventoryHack)
                    {
                        dragTarget.gameObject.GetComponent<InventoryHack>().AddHackToCard();
                    }
                    dragTarget = null;

                }

                isDragging = false;
                dragPlayspace = null;
            }
        
        yield return null;
        }
       
    }

    //Tries to play card
    public bool AttemptPlay(Card dragTarget, Playspace p)
    {
        if (dragTarget != null)
        {
            //tries to play card against the playscpace's parent gameobject enemy component
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
        print(to);
        print(from);
        if (to.allowedDonors.Contains(from)){
            to.NewPlayItem(moveTarget.gameObject, ((Card)moveTarget).CardSO);
            from.DestroyPlayItem(moveTarget);
        }

    }

    Vector2 MouseToWorldWithDistance(Vector3 screenPosition, GameObject target)
    {
        float distanceToPlane = Mathf.Abs(target.transform.position.z - Camera.main.transform.position.z);
        screenPosition.z = distanceToPlane;
        return Camera.main.ScreenToWorldPoint(screenPosition);
    }
}
