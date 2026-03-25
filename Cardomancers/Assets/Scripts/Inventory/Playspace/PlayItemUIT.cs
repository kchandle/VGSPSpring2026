using UnityEngine;
using UnityEngine.UIElements;

[UxmlElement]
public partial class PlayItemUIT : VisualElement
{
    public enum ItemType
    {
        CARD,
        HACK
    }

    public ScriptableObject data;

    public PlayItemUIT()
    {
        AddToClassList("play-item");
        this.AddManipulator(new CardDragInputUIT());
        var playspace = parent as PlayspaceUIT;
        playspace?.playItems.Add(this);
    }
}
