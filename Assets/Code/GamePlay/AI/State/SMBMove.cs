using UnityEngine;

public class SMBMove : LinkedSMB<MonsterStateMachine>
{

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monobeHaviour.monster.Move(monobeHaviour.target.transform.position);

        if (!monobeHaviour.TransAttack("isAttack", ref monobeHaviour.nomalAttack.attackDelay, monobeHaviour.nomalAttack.baseAttackDelay, monobeHaviour.nomalAttack.attackRange))
        {
            if (monobeHaviour.activeSkill != null)
            {
                monobeHaviour.TransAttack("isSkill", ref monobeHaviour.activeSkill.attackDelay, monobeHaviour.activeSkill.baseAttackDelay, monobeHaviour.activeSkill.attackRange);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }

}
