using UnityEngine;
using UnityEngine.UI;

namespace DialogueScripts
{

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] lines;
}

[System.Serializable]
public class DialogueLine
{
    // public string actorTag; // the tag of the Actor saying the line. Used for actors that have multiple instances across the map
    
    public Sprite talksprite; // image to be displayed in talksprite portrait during this dialogue line
    
    public string displayName; // name to be displayed during this dialogue line
    public float textDelay;
    [TextArea(3, 5)] public string text;
}
}