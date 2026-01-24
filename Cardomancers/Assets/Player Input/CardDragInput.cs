using UnityEngine;
using UnityEngine.InputSystem;

public class CardDragInput : MonoBehaviour
{

    public InputActionAsset inputActions; /// input actions

    private InputAction dragStart;
    private InputAction dragEnd;

    PlayItem dragTarget; // the current PlayItem being dragged (if any)

    public Vector3 dragTargetStartPos;
    public bool isDragging = false;


    void Awake()
    {
        dragStart = inputActions.Get
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // moves a PlayItem from one playSpace to another by destroying it and reinstancing it
    public void MoveToNewPlayspace(PlayItem moveTarget, Playspace from, Playspace to)
    {
        // only move the item if the "to" PlaySpace can receive PlayItems from the "from" PlaySpace
        if (from.allowedDonors.Contains(from)){
            to.NewPlayItem(moveTarget.gameObject);
            from.DestroyPlayItem(moveTarget);
        }

    }
}
