using UnityEngine;

public class DeadAIState : AIState
{
    public DeadAIState(AIBrain _brain) { brain = _brain; }

    public override void EnterState()
    {

    }

    public override void ExitState()
    {
        string poolType = brain.gameObject.name.Replace("(Clone)", "");
        brain.GetSpanwer().objectPools.GetPool(poolType).ReturnObject(brain.gameObject);
    }

    public override void Initialize()
    {

    }

    public override bool IsEligible()
    {
        if (brain.possessed.Stats.GetStat(EStatType.CurHealth) <= 0)
            return true;

        return false;
    }

    public override void UpdateState()
    {

    }
}
