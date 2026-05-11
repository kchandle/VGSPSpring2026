using System.Collections;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class InteractCollision : MonoBehaviour
{
     public UnityEvent interactable;

    [SerializeField] ParticleSystem particles;
    [SerializeField] AudioClip interactClip;
    public static AudioSource currentClip;

    private void Awake()
    {
        GetComponent<Collider>().isTrigger = true;
    }

    void OnTriggerEnter(Collider other)
    {
        //check the state, if speaking, inventory, or battle, return, if not it invokes

        if (other.tag == "Player")
        {
            if (GameStateScript.CurrentState == GameStateScript.GameState.INVENTORY) return;
            if (GameStateScript.CurrentState == GameStateScript.GameState.BATTLE) return;
            if (GameStateScript.CurrentState == GameStateScript.GameState.SPEAKING) return;

            Instantiate(particles, transform.position, Quaternion.identity);
            interactable.Invoke();
            if (interactClip != null)
            {
                if (currentClip != null) Destroy(currentClip);
                currentClip = SoundEffectManager.Instance.PlaySoundFXClip(interactClip, transform, 0.5f);

            }
        }
    }
}
