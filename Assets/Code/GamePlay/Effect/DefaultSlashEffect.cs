using UnityEngine;

public class DefaultSlashEffect : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnAnimationFinished()
    {
        DeactivateEffect();
    }

    public void ActivateEffect(TransformData transformData, bool bUpper)
    {
        gameObject.SetActive(true);
        gameObject.transform.position = transformData.position;
        gameObject.transform.rotation = transformData.rotation;


        if (bUpper)
        {
            animator.SetTrigger("UpperSlash");
        }
        else
        {
            animator.SetTrigger("LowerSlash");
        }
    }

    public void DeactivateEffect()
    {
        gameObject.SetActive(false);
    }
}
