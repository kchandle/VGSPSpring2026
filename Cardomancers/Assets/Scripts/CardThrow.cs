using UnityEngine;
using UnityEngine.InputSystem;

public class CardThrow : MonoBehaviour
{
    //the object pool the throw is getting objects from
    [SerializeField] ObjectPool pool; 

    [SerializeField] InputActionReference click;
    //where the mouse is on the screen
    private Vector2 mouseScreenPosition;

    [SerializeField] float cardForce;

    void Update()
    {
        //updates where the mouse is on the screen
        mouseScreenPosition = Mouse.current.position.ReadValue();
    }

    private void OnEnable()
    {
        click.action.started += OnThrow;
    }

    private void OnDisable()
    {
        click.action.started -= OnThrow;
    }

    public void OnThrow(InputAction.CallbackContext obj)
    {
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100f, ~LayerMask.GetMask("Player")))
        {
            GameObject card = pool.GetInstance();
            if (!card.TryGetComponent(out Rigidbody rb))
            {
                card.AddComponent<Rigidbody>();
                card.GetComponent<Rigidbody>().useGravity = false;

            }
            card.transform.position = transform.position;
            card.SetActive(true);
            Rigidbody cardRB = card.GetComponent<Rigidbody>();
            cardRB.linearVelocity = Vector3.zero;
            cardRB.angularVelocity = Vector3.zero;
            Vector3 cardDir = (hit.point - transform.position).normalized;
            cardDir = Vector3.ProjectOnPlane(cardDir, Vector3.up);
            cardRB.linearVelocity = cardDir * cardForce;
        }
    }
}
