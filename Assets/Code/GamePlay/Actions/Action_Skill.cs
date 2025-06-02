using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ESkillTarget
{
    Self,
    Single,
    Multi
}

/* AttackAction */
public class Action_Skill : BaseAction
{
    [SerializeField]
    protected ESkillTarget SkillTarget = ESkillTarget.Self;

    [SerializeField, ShowIf("SkillTarget", ESkillTarget.Multi)]
    protected int targetCount = 1;

    [SerializeField]
    protected GameObject EffectPrefab;

    [SerializeField]
    protected float transitionDelay = 5f;

    [SerializeField]
    protected float attackRange = 1f;
    public AnimationClip clip;

    protected TargetingComponent targetingComponent;

    public int TargetCount => targetCount;
    public float AttackRange => attackRange;
    public float TransitionDelay => transitionDelay;
    public float BaseDelay { get; protected set; }
    public bool IsRunning { get { return enabled; } }

    protected override void Awake()
    {
        base.Awake();
        targetingComponent = GetComponentInChildren<TargetingComponent>();

        BaseDelay = transitionDelay;
        enabled = false;
    }

    protected override void Update()
    {
        transitionDelay -= Time.deltaTime;

        if (transitionDelay <= 0.0f)
        {
            transitionDelay = BaseDelay;
            enabled = false;
        }
    }

    public override bool CanExcute()
    {
        if (enabled || !targetingComponent || !targetingComponent.HasNearTarget())
            return false;

        if (Vector2.Distance(targetingComponent.GetNearestTarget().transform.position, transform.position) > attackRange)
            return false;

        return true;
    }

    public override void ExcuteAction() 
    {
        enabled = true;

        if (EffectPrefab == null)
            return;

        IReadOnlyList<TargetElement> targets = targetingComponent.GetTargetList();
        switch (SkillTarget)
        {
            case ESkillTarget.Self:
                {
                    GameObject obj = Instantiate(EffectPrefab);
                    Effect2D instance = obj.GetComponent<Effect2D>();
                    obj.SetActive(true);
                    instance.ActivateEffect(gameObject);
                    break;
                }
            case ESkillTarget.Single:
                {
                    GameObject obj = Instantiate(EffectPrefab);
                    Effect2D instance = obj.GetComponent<Effect2D>();
                    obj.SetActive(true);
                    instance.ActivateEffect(gameObject, targets[0].target.transform);
                    break;
                }
            case ESkillTarget.Multi:
                for(int i = 0; i < TargetCount; ++i)
                {
                    if (targets.Count < i)
                        break;

                    GameObject obj = Instantiate(EffectPrefab);
                    Effect2D instance = obj.GetComponent<Effect2D>();
                    obj.SetActive(true);
                    instance.ActivateEffect(gameObject, targets[i].target.transform);
                }
                break;
        }
    }
}