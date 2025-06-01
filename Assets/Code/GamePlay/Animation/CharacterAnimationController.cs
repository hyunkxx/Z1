using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private Character character;
    private Animator animator;
    private Rigidbody2D rg2d;

    public void Awake()
    {
        character = GetComponent<Character>();
        animator = transform.Find("Root").GetComponent<Animator>();
        rg2d = GetComponent<Rigidbody2D>();
    }
    public void LateUpdate()
    {
        if(rg2d.linearVelocity.magnitude >= 0.1f)
        {
            animator.SetBool("isMove", true);
        }
        else
        {
            animator.SetBool("isMove", false);
        }
    }
}
