using UnityEngine;

public class SMBMonsterIdle : LinkedSMB<MonsterStateMachine>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (monobeHaviour == null) return;
        
        monobeHaviour.ActionType = null;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (monobeHaviour == null) return;

        if (!monobeHaviour.TransAttack(monobeHaviour.nomalAttack,"isAttack"))
        {
            if (monobeHaviour.AttackType != null)
            {
                monobeHaviour.TransAttack(monobeHaviour.skill[0], "isSkill");
            }
        }

        monobeHaviour.TransMove();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
