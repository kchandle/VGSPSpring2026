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
        //adds the OnThrow method to the input action for clicking
        click.action.started += OnThrow;
    }

    private void OnDisable()
    {
        //removes the OnThrow method to the click action whenever the script is destroyed or disabled
        click.action.started -= OnThrow;
    }

    public void OnThrow(InputAction.CallbackContext obj)
    {
        //gets a ray from the camera position to the mouse position on the screen
        Ray ray = Camera.main.ScreenPointToRay(mouseScreenPosition);
        RaycastHit hit;

        //casts the ray with a max range of 100 and ignores the Player layermask
        if (Physics.Raycast(ray, out hit, 100f, ~LayerMask.GetMask("Player")))
        {
            //gets the next card instance in the object pool
            GameObject card = pool.GetInstance();

            //runs if the new card instance doesnt have a rigidbody from destroying it whenever it hits something
            if (!card.TryGetComponent(out Rigidbody rb))
            {
                //adds a rigidbody and sets its gravity to false
                card.AddComponent<Rigidbody>();
                card.GetComponent<Rigidbody>().useGravity = false;

            }

            //resets the card instance position, sets it to actives and gets the rigidbody
            card.transform.position = transform.position;
            card.SetActive(true);
            Rigidbody cardRB = card.GetComponent<Rigidbody>();

            //resets the rigidbody linear and angular velocity
            cardRB.linearVelocity = Vector3.zero;
            cardRB.angularVelocity = Vector3.zero;

            //gets the normal direction based off the player position towards the mouse hit position
            Vector3 cardDir = (hit.point - transform.position).normalized;

            //flatens the direction to ignore the y axis and adds force to the card in that direction
            cardDir = Vector3.ProjectOnPlane(cardDir, Vector3.up);
            cardRB.linearVelocity = cardDir * cardForce;
        }
    }
}
