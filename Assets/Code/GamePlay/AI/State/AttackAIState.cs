using UnityEngine;

public class AttackAIState : AIState
{
    public AttackAIState(AIBrain _brain) { brain = _brain; }

    public override void Initialize()
    {
    }

    public override void EnterState()
    {
        ActionComponent actionComponent = brain.possessed.ActionComponent;
        actionComponent.TryExecute(EActionType.ATTACK);
    }

    public override void ExitState()
    {
    }

    public override bool IsEligible()
    {
        //if(TryExcute())
        // 어떤 조건에서 Attack State 를 들어오게 할지

        return true;
    }

    public override void UpdateState()
    {
    }
}
