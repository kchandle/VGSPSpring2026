using UnityEngine;
using UnityEngine.UIElements;

public class CardDragInputUIT : PointerManipulator
{
    private Vector2 start;
    private bool dragging;

    protected override void RegisterCallbacksOnTarget()
    {
        target.RegisterCallback<PointerDownEvent>(OnDown);
        target.RegisterCallback<PointerUpEvent>(OnUp);
        target.RegisterCallback<PointerMoveEvent>(OnMove);
    }

    protected override void UnregisterCallbacksFromTarget()
    {
        target.UnregisterCallback<PointerDownEvent>(OnDown);
        target.UnregisterCallback<PointerUpEvent>(OnUp);
        target.UnregisterCallback<PointerMoveEvent>(OnMove);
    }
    
    void OnDown(PointerDownEvent evt)
    {
        start = evt.position;
        dragging = true;
        target.BringToFront();
        target.CapturePointer(evt.pointerId);
    }

    void OnMove(PointerMoveEvent evt)
    {
        if (!dragging) return;

        target.style.translate = evt.position; 
        target.style.position = Position.Absolute;
    }

    void OnUp(PointerUpEvent evt)
    {
        dragging = false;
        target.ReleasePointer(evt.pointerId);
        target.style.translate = start;
        var ps = target.parent as PlayspaceUIT;
        ps?.UpdateLayout();
    }
}
