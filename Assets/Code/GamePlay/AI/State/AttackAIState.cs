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
        // � ���ǿ��� Attack State �� ������ ����

        return true;
    }

    public override void UpdateState()
    {
    }
}
