using UnityEngine;

public class SMBIdle : LinkedSMB<StateMachine>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        monobeHaviour.ActionType = null;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!monobeHaviour.TransAttack(monobeHaviour.nomalAttack,"isAttack", ref monobeHaviour.nomalAttack.attackDelay, monobeHaviour.nomalAttack.baseAttackDelay, monobeHaviour.nomalAttack.attackRange))
        {
            if (monobeHaviour.AttackType != null)
            {
                monobeHaviour.TransAttack(monobeHaviour.skill[0], "isSkill", ref monobeHaviour.AttackType.attackDelay, monobeHaviour.AttackType.baseAttackDelay, monobeHaviour.AttackType.attackRange);
            }
        }

        monobeHaviour.TransMove();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
