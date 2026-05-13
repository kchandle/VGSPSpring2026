using DialogueScripts;
using UnityEngine;

public class Cutscene : MonoBehaviour
{
    public Transform[] cutsceneCameraMoveTransforms;
    public Transform[] cutscenePlayerMoveTransforms;

    public void PassCutsceneTransforms()
    {
        DialogueManager.instance.cameraMoveTransforms = new(cutsceneCameraMoveTransforms);
        DialogueManager.instance.playerMoveTransforms = new(cutscenePlayerMoveTransforms);
    }
}