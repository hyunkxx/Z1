using UnityEngine;

public class SMBSpecialAttack : LinkedSMB<MonsterStateMachine>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monobeHaviour.ActionType = monobeHaviour.specailAttack;
        monobeHaviour.Action();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monobeHaviour.CheckAnimationEnd("TestSpecialAttack", "isSpecialAttack");
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        Debug.Log("Exit Attack");
    }
}
