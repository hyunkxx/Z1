using UnityEngine;
using UnityEngine.U2D;


public abstract class EffectBase : Z1Behaviour
{
    public abstract void ActivateEffect(GameObject effectOwner);
    public abstract void ActivateEffect(GameObject effectOwner, Transform trans);
    public abstract void ActivateEffect(GameObject effectOwner, TransformData trans);
    public abstract void ActivateEffect(GameObject effectOwner, Vector3 position, Quaternion rotation);
}

public class Effect2D : EffectBase
{
    [Header("Effect2D Property")]
    protected Animator animator;
    protected GameObject owner;
    protected SpriteRenderer sprite;

    protected override void Awake()
    {
        base.Awake();

        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        AddLastKeyframeEvent();
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        EffectDeactivated();
    }
    private void AddLastKeyframeEvent()
    {
        RuntimeAnimatorController controller = animator.runtimeAnimatorController;
        if (controller == null)
            return;

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
        EffectDeactivated();
    }

    public sealed override void ActivateEffect(GameObject effectOwner)
    {
        owner = effectOwner;

        transform.position = effectOwner.transform.position;
        transform.rotation = effectOwner.transform.rotation;
        transform.localPosition = effectOwner.transform.localPosition;

        EffectActivated();
    }
    public sealed override void ActivateEffect(GameObject effectOwner, Transform trans)
    {
        owner = effectOwner;

        transform.position = trans.position;
        transform.rotation = trans.rotation;

        EffectActivated();
    }
    public sealed override void ActivateEffect(GameObject effectOwner, TransformData trans)
    {
        owner = effectOwner;

        transform.position = trans.position;
        transform.rotation = trans.rotation;

        EffectActivated();
    }
    public sealed override void ActivateEffect(GameObject effectOwner, Vector3 position, Quaternion rotation)
    {
        owner = effectOwner;

        transform.position = position;
        transform.rotation = rotation;

        EffectActivated();
    }

    /* override this function to implement polymorphic behavior on activation and deactivation. */
    protected virtual void EffectActivated() { }
    protected virtual void EffectDeactivated() { Destroy(gameObject); }
}
