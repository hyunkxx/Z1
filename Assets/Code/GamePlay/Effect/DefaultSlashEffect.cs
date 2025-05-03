using UnityEngine;

public class DefaultSlashEffect : MonoBehaviour
{
    private Animator animator;
    private DamageProvider damageProvider;
    public DamageProvider DamageProvider => damageProvider;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        damageProvider = GetComponent<DamageProvider>();
    }

    public void OnAnimationFinished()
    {
        DeactivateEffect();
    }

    public void ActivateEffect(Character character, TransformData transformData, bool bUpper)
    {
        gameObject.SetActive(true);
        gameObject.transform.position = transformData.position;
        gameObject.transform.rotation = transformData.rotation;
        damageProvider.ActivateProvider(character.gameObject, character.Stats);

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
