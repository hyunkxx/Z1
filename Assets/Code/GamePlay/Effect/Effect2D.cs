using System;
using UnityEngine;
using UnityEngine.U2D;


public abstract class EffectBase : Z1Behaviour {}

public class Effect2D : EffectBase
{
    [Header("Effect2D Property")]
    protected Animator animator;
    protected GameObject owner;
    protected SpriteRenderer sprite;

    public event Action OnAnimmationFinished;
    public event Action<string> OnAnimationEvent;

    protected override void Awake()
    {
        sprite = GetComponentInChildren<SpriteRenderer>();
        animator = GetComponent<Animator>();
        AddLastKeyframeEvent();
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
                if (Mathf.Approximately(elem.time, clip.length) && elem.functionName == "AnimationFinished")
                {
                    hasEndEvent = true;
                    break;
                }
            }

            if (!hasEndEvent)
            {
                AnimationEvent animationEvent = new AnimationEvent();
                animationEvent.functionName = "AnimationFinished";
                animationEvent.time = clip.length;

                if (!clip.isLooping && !clip.name.Contains("__preview__"))
                {
                    clip.AddEvent(animationEvent);
                }
            }
        }
    }

    public bool IsValidFinishEvent()
    {
        return OnAnimmationFinished != null;
    }

    /* call from animation event */
    public virtual void AnimationFinished()
    {
        OnAnimmationFinished?.Invoke();
    }

    public void AnimationEvent(string param)
    {
        OnAnimationEvent?.Invoke(param);
    }
}
