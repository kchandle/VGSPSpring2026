using UnityEngine;
using UnityEngine.UI;

namespace DialogueScripts
{

[CreateAssetMenu(fileName = "DialogueSO", menuName = "Dialogue/DialogueSO")]
public class DialogueSO : ScriptableObject
{
    public DialogueLine[] lines;
    public bool startsBattle;

    public Color textColorDefault = new Color(1f, 1f, 1f, 1f);
    public Color textBackgroundColorDefault = new Color(1f, 1f, 1f, 1f);

    public Color titleBackgroundColorDefault = new Color(1f, 1f, 1f, 1f);
    public Color titleTextColorDefault = new Color(1f, 1f, 1f, 1f);

    public Color spriteColorDefault = new Color(1f, 1f, 1f, 1f);
    public Color spriteBorderColorDefault = new Color(1f, 1f, 1f, 1f);
}

[System.Serializable]
public class DialogueLine
{
    // public string actorTag; // the tag of the Actor saying the line. Used for actors that have multiple instances across the map
    
    public Sprite talksprite; // image to be displayed in talksprite portrait during this dialogue line
    
    public string displayName; // name to be displayed during this dialogue line
    public float textDelay;


    [TextArea(3, 5)] public string text;

        [Header("Cutscenes")]
        public bool lineHasCutscene;
        public Vector3 playerMovePosition;
        public Vector3 playerMoveRotation;
        public Vector3 cameraMovePosition;
    public Vector3 cameraRotation;
}
}