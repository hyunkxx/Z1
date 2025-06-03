using UnityEngine;

public class SMBMonsterMove : LinkedSMB<MonsterStateMachine>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (monobeHaviour.target && monobeHaviour.AttackType)
        {
            if (monobeHaviour.AttackType.AttackRange > monobeHaviour.targetDistance)
                monobeHaviour.TransIdle();
            else
                monobeHaviour.TransMoveToLocation();
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
