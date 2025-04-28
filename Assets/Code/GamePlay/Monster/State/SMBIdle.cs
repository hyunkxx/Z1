using UnityEngine;

public class SMBIdle : LinkedSMB<MonsterStateMachine>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monobeHaviour.ActionType = null;
        Debug.Log("Idle");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


        if (!monobeHaviour.TransAttack("isAttack", ref monobeHaviour.nomalAttack.attackDelay, monobeHaviour.nomalAttack.baseAttackDelay))
        {

        }
        else if (monobeHaviour.specailAttack != null)
        {
            if (monobeHaviour.TransAttack("isSpecialAttack", ref monobeHaviour.specailAttack.attackDelay, monobeHaviour.specailAttack.baseAttackDelay))
            {

            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
