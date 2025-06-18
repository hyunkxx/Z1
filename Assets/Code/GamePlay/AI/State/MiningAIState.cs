using System.Collections;
using UnityEngine;

public class MiningAIState : AIState
{
    public MiningAIState(AIBrain _brain) { brain = _brain; }

    //private float collectionAmount = 0f;
    //private float maxCollectionAmount = 100f;

    public override void EnterState()
    {
        TakeMineral();
    }

    public override void ExitState()
    {
        //brain.Target.GetComponent<Damageable>().OnDamageTaken() += TakeMineral;
    }

    public override void Initialize()
    {
    }

    public override bool IsEligible()
    {
        ActionComponent actionComponent = brain.possessed.ActionComponent;

        if (actionComponent.TryExecute(EActionType.ATTACK))
            return true;

        return false;
    }

    public override void UpdateState()
    {
    }

    private void TakeMineral()
    {
        //Debug.Log($"¹Ì³×¶ö Ã¤Áý : {brain.possessed.Stats.}");
       //Player.Mineral += brain.possessed.Stats.Damage;
    }
}
