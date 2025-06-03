using UnityEngine;

public class MoveAIState : AIState
{
    enum MoveState
    {
        None,
        MoveDirection,
        Purchase,
    }

    AIBrain aiBrain;
    MoveState CurMoveState = MoveState.None;

    public MoveAIState(AIBrain _brain) { aiBrain = _brain; }

    public override void Awake()
    {

    }

    public override void Start()
    {

    }

    public override void Update()
    {
        MoveState newMoveState = ChooseMoveState();

        if (CurMoveState != newMoveState)
        {
            CurMoveState = newMoveState;
            Move();
        }
    }

    public override void Initialize()
    {

    }

    MoveState ChooseMoveState()
    {
        return aiBrain.Target == null ? MoveState.MoveDirection : MoveState.Purchase;
    }

    void Move()
    {
        switch (CurMoveState)
        {
            case MoveState.MoveDirection:
                Vector2 dir = aiBrain.AIType == AIType.Character ? Vector2.right : Vector2.left;
                aiBrain.movementComponent.MoveToDirection(dir);
                break;

            case MoveState.Purchase:
                aiBrain.movementComponent.MoveToLocation(aiBrain.Target.transform.position);
                break;
        }
    }

    public override bool IsEligible() 
    {
        // 플레이어에게 일정 거리만큼 다가갔을때 Move False
        if (aiBrain.FindTarget() && aiBrain.targetDistanceSqr > aiBrain.DetectRange * aiBrain.DetectRange)
            return false;

        return true;
    }
}
