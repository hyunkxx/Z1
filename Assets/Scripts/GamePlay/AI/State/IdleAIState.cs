using UnityEngine;

public class IdleAIState : AIState
{
    public IdleAIState(AIBrain _brain) { brain = _brain; }

    public override void Initialize()
    {
    }

    public override void EnterState()
    {
        ActionComponent actionComponent = brain.possessed.ActionComponent;
        actionComponent.TryExecute(EActionType.IDLE);
    }

    public override void ExitState()
    {
    }

    public override bool IsEligible()
    {
        return false;
    }

    public override void UpdateState()
    {
    }
}
