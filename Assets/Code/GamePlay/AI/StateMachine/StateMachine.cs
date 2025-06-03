using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public Animator animator;

    // Brain �� State ���� �������ְ� � �������� �����Ѵ�.
    // State ���� ���� ó��

    public virtual void Initialize()
    {
        animator = transform.GetComponentInChildren<Animator>();
    }

    public void TransIdle()
    {
        foreach (var param in animator.parameters)
        {
            if (param.type == AnimatorControllerParameterType.Bool)
            {
                animator.ResetTrigger(param.name);
            }
        }

        animator.Play("Idle");
    }

    public bool TransStateAnim(string _paramName)
    {
        animator.SetTrigger(_paramName);
        return true;
    }

    public void ChangeStateClip(AnimationClip clip)
    {
        AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        overrideController.GetOverrides(overrides);
    
        string targetClipName = "Attack";

        for (int i = 0; i < overrides.Count; i++)
        {
            if (overrides[i].Key.name == targetClipName)
            {
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, clip);
                break;
            }
        }

        overrideController.ApplyOverrides(overrides);
        animator.runtimeAnimatorController = overrideController;
    }
}
