using UnityEngine;
using UnityEngine.InputSystem;

public class AttackTutorial : MonoBehaviour
{

    public void Update()
    {
        if (Input.GetMouseButton(0))
        {
            gameObject.SetActive(false);
        }

    }
}
