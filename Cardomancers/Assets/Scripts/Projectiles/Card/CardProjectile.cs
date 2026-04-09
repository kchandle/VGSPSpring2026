using UnityEngine;

public class CardProjectile : MonoBehaviour
{
    [SerializeField] private AudioClip[] cardHitSounds;

    private void OnCollisionEnter(Collision collision)
    {
        SoundEffectManager.Instance.PlaySoundFXClip(cardHitSounds, transform, 1f);
        //whenever the card hits something, if the object that was hit has the CardInteractable script 
        if (collision.gameObject.TryGetComponent(out CardInteractable interactable))
        {
            //calls the OnCardHit UnityEvent on the hit object and sets this object to false
            interactable.OnCardHit.Invoke();
            gameObject.SetActive(false);
        }

        //destroys the rigidbody if the card hits anything even without the CardInteractable
        Destroy(gameObject.GetComponent<Rigidbody>());
    }
}