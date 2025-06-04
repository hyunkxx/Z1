using UnityEngine;

public class MoveAIState : AIState
{
    enum MoveState
    {
        None,
        MoveDirection,
        Purchase,
    }

    MoveState CurMoveState = MoveState.None;

    public MoveAIState(AIBrain _brain) { brain = _brain; }

    public override void Initialize() { }
    public override void EnterState()
    {
        ActionComponent actionComponent = brain.possessed.ActionComponent;
        actionComponent.TryExecute(EActionType.MOVE);
    }

    public override void UpdateState()
    {
        MoveState newMoveState = ChooseMoveState();

        if (CurMoveState != newMoveState)
        {
            CurMoveState = newMoveState;
            Move();
        }
    }

    public override void ExitState()
    {
    }

    MoveState ChooseMoveState()
    {
        return brain.Target == null ? MoveState.MoveDirection : MoveState.Purchase;
    }

    void Move()
    {
        switch (CurMoveState)
        {
            case MoveState.MoveDirection:
                Vector2 dir = brain.AIType == AIType.Character ? Vector2.right : Vector2.left;
                brain.movementComponent.MoveToDirection(dir);
                break;

            case MoveState.Purchase:
                brain.movementComponent.MoveToLocation(brain.Target.transform.position);
                break;
        }

        /**/
        ActionComponent actionComponent = brain.possessed.ActionComponent;
        actionComponent.TryExecute(EActionType.ATTACK);
    }

    public override bool IsEligible() 
    {
        // 플레이어에게 일정 거리만큼 다가갔을때 Move False
        if (brain.FindTarget() && brain.targetDistanceSqr > brain.DetectRange * brain.DetectRange)
            return false;

        return true;
    }
}
