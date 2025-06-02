using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class TempStateMachine : MonoBehaviour
{
    public Animator Animator;
    public MovementComponent Movement;
    public TargetingComponent TargetingComponent;

    //[HideInInspector] public GameObject target;

    public BaseAction CurrentAction { get; private set; }
    public List<BaseAction> ActionSet;

    public GameObject Target => TargetingComponent ? TargetingComponent.GetNearestTarget() : null;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        Movement = GetComponent<MovementComponent>();
        TargetingComponent = GetComponentInChildren<TargetingComponent>();
    }

    void Start()
    {
        LinkedSMB<TempStateMachine>.Initialize(Animator, this);
    }

    ////Test
    //private void Update()
    //{
    //    //
    //    //
    //    //
    //    //
    //    //

    //}
}
