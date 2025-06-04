using System;
using System.Collections.Generic;
using UnityEngine;

enum AIStateType
{
    Idle,
    Move,
    Attack,
}

public enum AIType
{
    Character,
    Monster,
}

public class AIBrain : MonoBehaviour
{
    AIStateType[] AIStates = (AIStateType[])Enum.GetValues(typeof(AIStateType));
    AIStateType CurrentState;
    [SerializeField] AIType aIType;
    Dictionary<AIStateType, AIState> Logics;

    public Character possessed { get; private set; }
    public GameObject Target { get; private set; }

    public MovementComponent movementComponent { get; private set; }
    TargetingComponent targetingComponent;
    StateMachine stateMachine;

    public float DetectRange;
    public float targetDistanceSqr = 9999;

    public AIType AIType => aIType;

    public void Initialize(Character character, AIType type, float detectRange)
    {
        possessed = character;
        aIType = type;
        DetectRange = detectRange;
        stateMachine = character.GetComponent<StateMachine>();
    }

    private void Awake()
    {
        Logics = new Dictionary<AIStateType, AIState>()
                {
                    [AIStateType.Idle] = new IdleAIState(this),
                    [AIStateType.Attack] = new AttackAIState(this),
                    [AIStateType.Move] = new MoveAIState(this),
                };

        targetingComponent = transform.GetComponentInChildren<TargetingComponent>();
        movementComponent = transform.GetComponent<MovementComponent>();
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
        foreach (AIStateType aiStateType in AIStates)
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
}
