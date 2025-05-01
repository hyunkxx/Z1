using UnityEngine;

public class Effect : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckAnimationEnd(string _anim)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(_anim) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            Destroy(this);
    }
}
