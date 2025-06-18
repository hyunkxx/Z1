using System.Collections;
using UnityEngine;

public class MiningAIState : AIState
{
    public MiningAIState(AIBrain _brain) { brain = _brain; }

    public int collectAmount = 0;
    int maxCollectAmount = 15;

    ParticleSystem MiningParticle;

    public override void EnterState()
    {
    }

    public override void ExitState()
    {
    }

    public override void Initialize()
    {
        MiningParticle = brain.GetComponentInChildren<ParticleSystem>();
    }

    public override bool IsEligible()
    {
        if (collectAmount >= maxCollectAmount) 
        { 
            collectAmount = maxCollectAmount;
            MoveMiningAIState aIState = (MoveMiningAIState)brain.GetLogic(AIStateType.MoveMining);
            brain.SetTarget(aIState.ComandCenter.gameObject);
            aIState.isFullAmount = true;
            brain.possessed.TargetingComponent.gameObject.SetActive(false);
            return false; 
        }

        ActionComponent actionComponent = brain.possessed.ActionComponent;

        if (actionComponent.TryExecute(EActionType.ATTACK))
        {
            TakeMineral();
            return true;
        }

        return false;
    }

    public override void UpdateState()
    {
    }

    private void TakeMineral()
    {
        MiningParticle.Play();
        collectAmount += (int)brain.possessed.Stats.GetStat(EStatType.AttackDamage);
    }
}
