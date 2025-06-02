using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterStateMachine : StateMachine
{

    private void Awake()
    {
        Initialize();
    }

    public override void Initialize()
    {
        base.Initialize();

        LinkedSMB<MonsterStateMachine>.Initialize(animator, this);
    }

    void Start()
    {
        StartCoroutine(FindTarget());
    }

    void Update()
    {

    }

    // ∆–≈œ
    public void SkillPatern()
    {

    }
}