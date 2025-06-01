using UnityEngine;

public class SMBCharacterMove : LinkedSMB<CharacterStateMachine>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (monobeHaviour.target)
            monobeHaviour.movement.MoveToLocation(monobeHaviour.target.transform.position);
        else
            monobeHaviour.movement.MoveToDirection(Vector2.left);

        if (!monobeHaviour.TransAttack(monobeHaviour.nomalAttack, "isAttack", ref monobeHaviour.nomalAttack.attackDelay, monobeHaviour.nomalAttack.baseAttackDelay, monobeHaviour.nomalAttack.attackRange))
        {
            if (monobeHaviour.AttackType != null)
            {
                monobeHaviour.TransAttack(monobeHaviour.skill[0], "isSkill", ref monobeHaviour.AttackType.attackDelay, monobeHaviour.AttackType.baseAttackDelay, monobeHaviour.AttackType.attackRange);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
