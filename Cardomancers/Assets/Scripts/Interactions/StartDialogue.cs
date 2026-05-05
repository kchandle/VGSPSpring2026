using DialogueScripts;
using UnityEngine;

public class StartDialogue : MonoBehaviour
{
    public DialogueSO dialogue_SO;
    Cutscene cutscene;

    public GameObject exclamationMark;
    [HideInInspector] public bool isShowingExclamation;

    public void Awake()
    {
        cutscene = GetComponent<Cutscene>();
        isShowingExclamation = dialogue_SO.hasExclamationMark;
    }

    public void OnEnable()
    {
        exclamationMark.SetActive(isShowingExclamation);
    }

    public void BeginDialogue()
    {
        cutscene.PassCutsceneTransforms();
        DialogueManager.instance.StartDialogue(dialogue_SO);
    }

    public void ToggleExclamation(bool value)
    {
        isShowingExclamation = value;
        exclamationMark.SetActive(isShowingExclamation);
    }
}