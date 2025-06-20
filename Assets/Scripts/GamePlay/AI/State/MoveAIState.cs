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
        }

        Move();
    }

    public override void ExitState()
    {
        brain.movementComponent.ResetMovement();
        CurMoveState = MoveState.None;
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
    }

    public override bool IsEligible() 
    {
        GameObject target = brain.Target? brain.Target : brain.FindTarget();
        if (target && brain.GetTargetDistance() < brain.DetectRange * brain.DetectRange)
        {
            return false;
        }

        return true;
    }
}
