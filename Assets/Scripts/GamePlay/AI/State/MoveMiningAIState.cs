using UnityEngine;

public class MoveMiningAIState : AIState
{
    public MoveMiningAIState(AIBrain _brain) { brain = _brain; }

    public bool isFullAmount = false;
    public Transform ComandCenter;
    Transform[] Mineral;
    DefenceGameRule gameRule;

    bool isInitialize = false;
    public override void EnterState()
    {
        if (!isInitialize)
        {
            DefenceSpawner spawner = (DefenceSpawner)brain.GetSpanwer();
            ComandCenter = spawner.ComandSpanwPos[0];
            Mineral = spawner.MineralSpawnPos;
            brain.SetTarget(Mineral[0].gameObject);
            gameRule = (DefenceGameRule)GameManager.Instance.GameMode.Rule;
            isInitialize = true;
        }

        ActionComponent actionComponent = brain.possessed.ActionComponent;
        actionComponent.TryExecute(EActionType.MOVE);
    }

    public override void UpdateState()
    {
        Move();
    }

    public override void ExitState()
    {
        brain.movementComponent.ResetMovement();
    }

    public override void Initialize()
    {
    }

    public override bool IsEligible()
    {
        if (isFinishedAnim()) return false;

        if (brain.Target && brain.GetTargetDistance() < brain.DetectRange * brain.DetectRange)
        {
            if (isFullAmount)
            {
                MiningAIState aIState = (MiningAIState)brain.GetLogic(AIStateType.Mining);
                gameRule.HaveGreenStoneCount += aIState.collectAmount;
                aIState.collectAmount = 0;
                isFullAmount = false;
                brain.SetTarget(Mineral[0].gameObject);
                brain.possessed.TargetingComponent.gameObject.SetActive(true);
            }
            return false;
        }

        return true;
    }

    bool isFinishedAnim()
    {
        AnimatorStateInfo stateInfo = brain.possessed.Animator.GetCurrentAnimatorStateInfo(0); // 0Àº Base Layer
        if (stateInfo.IsName("Attack"))
        {
            return true;
        }

        return false;
    }

    void Move()
    {
        brain.movementComponent.MoveToLocation(brain.Target.transform.position);
    }
}
