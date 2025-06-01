using UnityEngine;

public class SMBCharacterIdle : LinkedSMB<CharacterStateMachine>
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
            if (monobeHaviour.TransAttack(monobeHaviour.skill[1], "isSkill"))
            {

            }
        }

        monobeHaviour.TransMove();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
