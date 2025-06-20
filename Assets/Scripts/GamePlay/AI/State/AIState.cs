using System;
using UnityEngine;

public abstract class AIState
{
    public AIBrain brain { get; protected set; }

    abstract public void Initialize();

    abstract public void EnterState();
    abstract public void UpdateState();
    abstract public void ExitState();

    abstract public bool IsEligible();
}
