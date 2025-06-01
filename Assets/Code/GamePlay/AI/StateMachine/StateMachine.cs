using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public IAction ActionType;
    public Animator animator;
    public MovementComponent movement;
    public TargetingComponent targetingComponent;

    [HideInInspector] public GameObject target;
     public AttackAction AttackType;
    public AttackAction nomalAttack;
    public AttackAction[] skill;

    private void Awake()
    {

    }

    public virtual void Initialize()
    {
        movement = GetComponent<MovementComponent>();
        animator = transform.GetComponentInChildren<Animator>();
        nomalAttack = GetComponent<AttackAction>();
        skill = GetComponents<AttackAction>();
        targetingComponent = transform.GetComponentInChildren<TargetingComponent>();
    }

    public void TransMove()
    {
        animator.SetBool("isMove", true);
    }

    public bool TransAttack(AttackAction _ationType, string _paramName)
    {
        if (target == null) return false;
        if (_ationType.attackDelay > 0f) return false;
        if (Vector2.Distance(target.transform.position, transform.position) > _ationType.attackRange) return false;
    
        AttackType = _ationType;
        _ationType.attackDelay = _ationType.baseAttackDelay;
        animator.SetTrigger(_paramName);
        animator.SetBool("isMove", false);
        return true;
    }

    public void ChangeStateClip()
    {
        AnimatorOverrideController overrideController = new AnimatorOverrideController(animator.runtimeAnimatorController);

        var overrides = new List<KeyValuePair<AnimationClip, AnimationClip>>();
        overrideController.GetOverrides(overrides);
    
        string targetClipName = "Skill";

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

    public void Action()
    {
        if (AttackType == null) return;
    
        ActionType.ExcuteAction();
    }

    public void FindTarget()
    {
        if (targetingComponent == null) return;

        GameObject nearestTarget = targetingComponent.GetNearestTarget();

        if (nearestTarget != null)
            target = nearestTarget;
    }
}
