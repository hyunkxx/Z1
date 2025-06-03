using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public Animator animator;
    public MovementComponent movement;
    public TargetingComponent targetingComponent;

    [HideInInspector] public GameObject target;
    [HideInInspector] public float targetDistance;
    public Action_Skill AttackType;

    public virtual void Initialize()
    {
        movement = GetComponent<MovementComponent>();
        animator = transform.GetComponentInChildren<Animator>();
        targetingComponent = transform.GetComponentInChildren<TargetingComponent>();
        targetDistance = 9999;
    }

    public void TransMoveToLocation()
    {
        if (AttackType && AttackType.AttackRange > targetDistance) return;

        movement.MoveToLocation(target.transform.position);
        animator.SetBool("isMove", true);
    }

    public void TransMoveToDirection(Vector2 direction)
    {
        if (AttackType && AttackType.AttackRange > targetDistance)
        {
            return;
        }

        movement.MoveToDirection(direction);
        animator.SetBool("isMove", true);
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

        movement.ResetMovement();
        animator.Play("Idle");
    }

    public bool TransAttack(Action_Skill _ationType, string _paramName)
    {
        if (target == null) return false;
        if (_ationType == null) return false;
        if (_ationType.TransitionDelay > 0f) return false;
        if (Vector2.Distance(target.transform.position, transform.position) > _ationType.AttackRange) return false;
    
        AttackType = _ationType;
        animator.SetBool("isMove", false);
        animator.SetTrigger(_paramName);
        return true;
    }

    public void ChangeStateClip()
    {
        AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        overrideController.GetOverrides(overrides);
    
        string targetClipName = "Attack";

        for (int i = 0; i < overrides.Count; i++)
        {
            if (overrides[i].Key.name == targetClipName)
            {
                overrides[i] = new KeyValuePair<AnimationClip, AnimationClip>(overrides[i].Key, AttackType.clip);
                break;
            }
        }

        overrideController.ApplyOverrides(overrides);
        animator.runtimeAnimatorController = overrideController;
    }

    protected IEnumerator FindTarget()
    {
        while (gameObject.activeSelf)
        {
            if (targetingComponent == null) break;

            GameObject nearestTarget = targetingComponent.GetNearestTarget();

            if (nearestTarget != null)
            {
                target = nearestTarget;
                targetDistance = Vector2.Distance(transform.position, target.transform.position);
            }

            yield return new WaitForSeconds(0.5f);
        }
    }
}
