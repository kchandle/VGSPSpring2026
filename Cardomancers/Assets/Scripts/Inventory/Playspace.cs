using UnityEngine;
using System.Collections.Generic;

public class Playspace : MonoBehaviour
{
    public PlayItem focusTarget; // the current PlayItem being highlighted
    public float focusOffset = 20f; // how much the focusTarget will be offset from non-focused PlayItem's

    List<PlayItem> playItems = new List<PlayItem>(); //All PlayItems currently in this PlaySpace

//PlaySpaces that this PlaySpace can accept PlayItems from
// For a PlayItem to move into this playspace, it must be FROM a PlaySpace in this list
    public List<Playspace> allowedDonors = new List<Playspace>(); 



    // Width and padding for the horizontal layout
    // alters how the PlayItems are spaced out
    public float width = 30f;
    public float padding = 15f;

    BoxCollider2D playArea;

    private PlayItem dragTarget; // the currently PlayItem being dragged
    private Vector3 dragTargetPosition; // position of the dragTarget PlayItem
    

    void Awake() => playArea = GetComponent<BoxCollider2D>();

    void Start()
    {

    }

    void Update()
    {
        // Get the focusTarget and dragTargets (if any)
        int targetIndex = playItems.IndexOf(focusTarget);

        HorizontalLayout(targetIndex);

        if (dragTarget) dragTarget.position = dragTargetPosition;

        focusTarget = null;
        dragTarget = null;
    }

    // Create a new PlayItem in this playspace
    public GameObject NewPlayItem(GameObject prefab)
    {
        GameObject newPlayItem = Instantiate(prefab);
        newPlayItem.transform.SetParent(transform);

        playItems.Add(newPlayItem.GetComponent<PlayItem>());
        return newPlayItem;
    }

    // Destroys a specific PlayItem in this PlaySpace
    public void DestroyPlayItem(PlayItem playItem)
    {
        if (!playItems.Contains(playItem)) return;
        playItems.Remove(playItem);
        Destroy(playItem.gameObject);
    }

// Arranges all play items in a line
    void HorizontalLayout(int targetIndex = -1)
    {
        int count = playItems.Count;
        float totalWidth = (count * width) + (count - 1) * padding;
        Vector3 start = transform.position + Vector3.left * (totalWidth / 2);
            for (int i = 0; i < count; i++)
            {
                float index = (float)i + 0.5f;
                Vector3 position = start + Vector3.right * ((index * width) + (i * padding));

                if (targetIndex != -1)
                {
                    if (i == targetIndex - 1) position += Vector3.left * focusOffset;
                    else if (i == targetIndex + 1) position += Vector3.right * focusOffset;
                    else if (i == targetIndex) position += Vector3.up * focusOffset;
                }
                
            playItems[i].position = position;
            }

    }
    
    // checks if a Vector3 positions it within the PlayArea's bounds
    public bool InPlayArea(Vector3 position)
    {
        return playArea.OverlapPoint(position);
    }

// If the player is hovering over this playspace, get the playItem closest to the player's cursor
// and highlight it
    public PlayItem GetNearestPlayItem(Vector3 position)
    {
        if (!InPlayArea(position)) return null;
        PlayItem target = null;
        float minDistance = 1000f;

        foreach (PlayItem p in playItems)
        {
            float distance = (position - p.transform.position).magnitude;
            if (distance <= minDistance)
            {
                minDistance = distance;
                target = p;
            }
        }
        return target;
    }

    // Sets the DragTarget. Can be used by other scripts for drag functionality
    public void SetDragTarget(PlayItem dragTarget, Vector3 dragTargetPosition)
    {
        this.dragTarget = dragTarget;
        this.dragTargetPosition = dragTargetPosition;
    }

}
