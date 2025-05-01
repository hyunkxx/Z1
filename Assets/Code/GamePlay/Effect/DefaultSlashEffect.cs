using UnityEngine;

public class DefaultSlashEffect : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ActivateSlash(TransformData transformData, bool bUpper)
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

    public void DeactivateSlash()
    {
        Debug.Log("slashEnd");
        gameObject.SetActive(false);
    }
}
