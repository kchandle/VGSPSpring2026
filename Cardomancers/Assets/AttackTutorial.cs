using UnityEngine;
using UnityEngine.InputSystem;

public class AttackTutorial : MonoBehaviour
{
    public InputActionReference clickAction;

    public void Update()
    {
        if (clickAction.action.IsPressed())
        {
            gameObject.SetActive(false);
        }

    }
}
