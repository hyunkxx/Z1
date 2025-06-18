using System;
using System.Collections.Generic;
using UnityEngine;

public enum AIStateType
{
    Dead,
    Idle,
    Attack,
    Mining,
    Move,
    MoveMining,
}

public enum AIType
{
    Character,
    Monster,
}

public class AIBrain : MonoBehaviour
{
    AIStateType CurrentState;
    [SerializeField] AIType aIType;
    Dictionary<AIStateType, AIState> Logics;
    AIStateSet StateSet;

    public Character possessed { get; private set; }
    public GameObject Target { get; private set; }

    public MovementComponent movementComponent { get; private set; }
    TargetingComponent targetingComponent;
    StateMachine stateMachine;
    SpawnController spawner;

    public float DetectRange;
    public float targetDistanceSqr = 9999;

    public AIType AIType => aIType;

    public SpawnController GetSpanwer() { return spawner; }
    public void SetSpawner(SpawnController _spawner) { spawner = _spawner; } 
    public void SetTarget(GameObject _target) { Target = _target; }
    public AIState GetLogic(AIStateType type) { return Logics[type]; }

    public void Initialize(Character character, AIType type, AIStateSet aIStateSet)
    {
        Logics = new Dictionary<AIStateType, AIState>();
        targetingComponent = transform.GetComponentInChildren<TargetingComponent>();
        movementComponent = transform.GetComponent<MovementComponent>();

        possessed = character;
        aIType = type;
        StateSet = aIStateSet;

        for (int i = 0; i < StateSet.aIStateSet.Count; ++i)
        {
            Logics.Add(StateSet.aIStateSet[i], CreateState(StateSet.aIStateSet[i]));
            Logics[StateSet.aIStateSet[i]].Initialize();
        }

        ActionComponent component = possessed.ActionComponent;
        ActionAbility ability = component.GetAction(EActionType.ATTACK) as ActionAbility;
        DetectRange = ability ? ability.TriggerDistance : 1f;

        stateMachine = character.GetComponent<StateMachine>();
    }

    private void Update()
    {
        AIStateType newState = FindBestEligibleAIState();

        if (CurrentState != newState)
        {
            CurrentState = newState;
            stateMachine.TransitionTo(Logics[CurrentState]);
        }
        Logics[CurrentState].UpdateState();
    }

    private AIStateType FindBestEligibleAIState()
    {
        foreach (AIStateType aiStateType in StateSet.aIStateSet)
        {
            if (Logics[aiStateType].IsEligible())
            {
                return aiStateType;
            }
        }

        return AIStateType.Idle;
    }

    public float GetTargetDistance() { return Target? (transform.position - Target.transform.position).sqrMagnitude : 9999f; }

    public GameObject FindTarget()
    {
        if (targetingComponent == null) return null;

        GameObject nearestTarget = targetingComponent.GetNearestTarget();

        if (nearestTarget != null)
        {
            Target = nearestTarget;
            targetDistanceSqr = (transform.position - Target.transform.position).sqrMagnitude;
            return Target;
        }

        return null;
    }

    AIState CreateState(AIStateType type)
    {
        switch (type)
        {
            case AIStateType.Idle:
                return new IdleAIState(this);
            case AIStateType.Move:
                return new MoveAIState(this);
            case AIStateType.Attack:
                return new AttackAIState(this);
            case AIStateType.Mining:
                return new MiningAIState(this);
            case AIStateType.MoveMining:
                return new MoveMiningAIState(this);
            case AIStateType.Dead:
                return new DeadAIState(this);
            default:
                return null;
        }
    }
}
