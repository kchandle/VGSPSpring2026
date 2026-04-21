using UnityEngine;

public class AnimateVFX : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Finished"))
        {
            Destroy(this.gameObject);
        }
    }
}