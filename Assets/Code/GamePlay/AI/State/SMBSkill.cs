using UnityEngine;

public class SMBSkill : LinkedSMB<MonsterStateMachine>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monobeHaviour.ChangeStateClip();
        monobeHaviour.ActionType = monobeHaviour.activeSkill;
        monobeHaviour.activeSkill.SetEffectTransform(monobeHaviour.target.transform);
        monobeHaviour.Action();
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
        Debug.Log("Exit Attack");
    }
}