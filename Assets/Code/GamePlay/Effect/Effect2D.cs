using UnityEngine;


public abstract class EffectBase : Z1Behaviour
{
    public abstract void ActivateEffect(GameObject effectOwner);
    public abstract void ActivateEffect(GameObject effectOwner, TransformData trans);
    public abstract void ActivateEffect(GameObject effectOwner, Vector3 position, Quaternion rotation);
}

public class Effect2D : EffectBase
{
    [Header("Effect2D Property")]
    [SerializeField] 
    private DamageProvider damageProvider;
    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private GameObject owner;

    protected override void Awake()
    {
        base.Awake();

        Collider2D collider = GetComponent<Collider2D>();
        Debug.Assert(collider != null, "Effect2D has no assigned Collider.");
        collider.isTrigger = true;

        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        AddLastKeyframeEvent();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EffectDeactivated();
    }

    public void CheckAnimationEnd(string _anim)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(_anim) && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1)
            Destroy(this);
    }
    private void AddLastKeyframeEvent()
    {
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;
        foreach (AnimationClip clip in controller.animationClips)
        {
            bool hasEndEvent = false;
            foreach (AnimationEvent elem in clip.events)
            {
                if (Mathf.Approximately(elem.time, clip.length) && elem.functionName == "OnAnimationEnd")
                {
                    hasEndEvent = true;
                    break;
                }
            }

            if (!hasEndEvent)
            {
                AnimationEvent animationEvent = new AnimationEvent();
                animationEvent.functionName = "OnAnimationFinished";
                animationEvent.time = clip.length;

                if (!clip.isLooping && !clip.name.Contains("__preview__"))
                {
                    clip.AddEvent(animationEvent);
                }
            }
        }
    }

    /* call from animation event */
    public virtual void OnAnimationFinished()
    {
        //or Destroy(gameObject);
        gameObject.SetActive(false);
    }
    /* call from animation event */
    public virtual void OnEnableDamageTrigger()
    {
        if (owner == null)
            return;

        CharacterStats stats = owner.GetComponent<CharacterStats>();
        damageProvider.ActivateProvider(owner, stats);
    }

    public sealed override void ActivateEffect(GameObject effectOwner)
    {
        owner = effectOwner;

        transform.position = effectOwner.transform.position;
        transform.rotation = effectOwner.transform.rotation;
        transform.localPosition = effectOwner.transform.localPosition;

        EffectActivated();
    }
    public sealed override void ActivateEffect(GameObject effectOwner, TransformData trans)
    {
        owner = effectOwner;

        transform.position = trans.position;
        transform.rotation = trans.rotation;
        transform.localPosition = trans.localPosition;

        EffectActivated();
    }
    public sealed override void ActivateEffect(GameObject effectOwner, Vector3 position, Quaternion rotation)
    {
        owner = effectOwner;

        transform.position = position;
        transform.rotation = rotation;
        transform.localPosition = Vector3.zero;

        EffectActivated();
    }

    /* override this function to implement polymorphic behavior on activation and deactivation. */
    protected virtual void EffectActivated() { gameObject.SetActive(true); }
    protected virtual void EffectDeactivated() { gameObject.SetActive(false); }
}
