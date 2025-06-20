using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public Animator animator;
    public AIState currentState { get; private set; }

    // Brain �� State ���� �������ְ� � �������� �����Ѵ�.
    // State ���� ���� ó��

    public void Awake()
    {
        animator = transform.GetComponentInChildren<Animator>();
    }

    public void TransitionTo(AIState newState)
    {
        if (currentState == newState)
            return;

        if (currentState != null)
        {
            currentState.ExitState();
        }

        currentState = newState;
        currentState.EnterState();
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
