using UnityEngine;

public class IdleAIState : AIState
{
    AIBrain aiBrain;

    public IdleAIState(AIBrain _brain) { aiBrain = _brain; }

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
        return false;
    }
}
