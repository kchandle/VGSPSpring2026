using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{

    [SerializeField] Animator animator;

    bool defeated;
    bool idle;
    bool stunned;
    bool attack;
    bool damaged;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(idle)
        {
            animator.GetBool("Idle");
        }
        else if(defeated)
        {
            animator.GetBool("Defeated");
        }
        else if(stunned)
        {
            animator.GetBool("Stunned");
        }
        else if(attack)
        {
            animator.GetBool("Attack");
        }
        else if(damaged)
        {
            animator.GetBool("Damaged");
        }
    }
}
