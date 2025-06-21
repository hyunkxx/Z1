using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AnimationHook : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> eventReceiver = new();
    private Animator animator;

    private void Awake()
    {
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

    public void AnimationFinished()
    {
        if(eventReceiver.Count > 0)
        {
            foreach(GameObject GO in eventReceiver)
            {
                GO.SendMessage("AnimationFinished");
            }
        }
    }
}
