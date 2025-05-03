using UnityEngine;

public class DefaultSlashEffect : MonoBehaviour
{
    private Animator animator;
    private DamageProvider damageProvider;
    private SpriteRenderer sprite;

    public DamageProvider DamageProvider => damageProvider;
    public SpriteRenderer Sprite => sprite;

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
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

        sprite.flipY = bUpper ? false : true;
        if(!character.IsRight())
        {
            sprite.flipY = !sprite.flipY;
        }

        animator.SetTrigger("UpperSlash");
    }

    public void DeactivateEffect()
    {
        gameObject.SetActive(false);
    }
}
