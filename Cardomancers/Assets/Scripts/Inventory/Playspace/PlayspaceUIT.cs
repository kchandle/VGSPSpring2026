using UnityEngine;
using UnityEngine.UIElements;
using System.Collections.Generic;

[UxmlElement]
public partial class PlayspaceUIT : VisualElement
{
    [UxmlAttribute] public List<PlayItemUIT> playItems = new List<PlayItemUIT>();
    [UxmlAttribute] public int maxItems = 50;
    [UxmlAttribute] public bool horizontalLayout = true;

    [UxmlAttribute] public int columnCount = 5;

    public static PlayItemUIT currentlyDragged;
    
    public List<PlayspaceUIT> allowedDonors = new List<PlayspaceUIT>();

    public PlayspaceUIT()
    {
        AddToClassList("PlayspaceUIT");
        UpdateLayout();
        RegisterCallback<PointerUpEvent>(evt =>
        {
            UpdateLayout();
        });
    }

    public void UpdateLayout()
    {
        EnableInClassList("playspace", true);
        EnableInClassList("playspace-grid", !horizontalLayout);
    }

    public bool CanAccept(PlayspaceUIT from)
    {
        if (allowedDonors.Contains(from)) return true;
        return false;
    }

    

}
