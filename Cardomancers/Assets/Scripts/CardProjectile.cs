using UnityEngine;

public class CardProjectile : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out CardInteractable interactable))
        {
            interactable.OnCardHit.Invoke();
            gameObject.SetActive(false);
        }
        Destroy(gameObject.GetComponent<Rigidbody>());
    }
}