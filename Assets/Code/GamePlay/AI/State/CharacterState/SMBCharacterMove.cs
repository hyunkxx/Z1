using UnityEngine;

public class SMBCharacterMove : LinkedSMB<CharacterStateMachine>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(monobeHaviour.target && monobeHaviour.AttackType)
        {
            if (monobeHaviour.AttackType.AttackRange > monobeHaviour.targetDistance)
                monobeHaviour.TransIdle();
        }
        //if (!monobeHaviour.TransAttack(monobeHaviour.nomalAttack, "isAttack"))
        //{
        //    if (monobeHaviour.AttackType != null)
        //    {
        //        monobeHaviour.TransAttack(monobeHaviour.skill[0], "isSkill");
        //    }
        //}
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
