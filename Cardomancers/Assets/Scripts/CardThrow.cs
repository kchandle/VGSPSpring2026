using UnityEngine;
using UnityEngine.InputSystem;

public class CardThrow : MonoBehaviour
{
    //the object pool the throw is getting objects from
    [SerializeField] ObjectPool pool; 

    //where the mouse is on the screen
    private Vector2 mouseScreenPosition;

    [SerializeField] float cardForce = 10f;

    [SerializeField] private AudioClip[] throwWooshSounds;
    [SerializeField] private AudioClip[] throwGruntSounds;

    void Update()
    {
        //updates where the mouse is on the screen
        mouseScreenPosition = Mouse.current.position.ReadValue();
    }

    public void OnCardFire(InputAction.CallbackContext context)
    {
        //returns if it isnt the frame that it is pressed
        if (!context.started) return;

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

            SoundEffectManager.Instance.PlaySoundFXClip(throwGruntSounds, transform, 1f);
            SoundEffectManager.Instance.PlaySoundFXClip(throwWooshSounds, transform, 1f);

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
