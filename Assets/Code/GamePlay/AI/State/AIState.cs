using System;
using UnityEngine;

public abstract class AIState
{
    abstract public void Initialize();

    abstract public void Awake();

    abstract public void Start();

    abstract public void Update();

    abstract public bool IsEligible();
}
