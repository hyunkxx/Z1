using UnityEngine;

public class AttackAIState : AIState
{
    AIBrain aiBrain;

    public AttackAIState(AIBrain _brain) { aiBrain = _brain; }

    public override void Awake()
    {

    }

    public override void Start()
    {

    }

    public override void Update()
    {

    }

    public override void Initialize()
    {

    }

    public override bool IsEligible()
    {
        //if(TryExcute())
        // 어떤 조건에서 Attack State 를 들어오게 할지

        return true;
    }
}
