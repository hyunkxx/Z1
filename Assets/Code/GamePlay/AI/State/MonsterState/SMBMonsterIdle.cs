using UnityEngine;

public class SMBMonsterIdle : LinkedSMB<MonsterStateMachine>
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (monobeHaviour == null) return;
        
        monobeHaviour.AttackType = null;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (monobeHaviour == null) return;

        monobeHaviour.TransAttack(monobeHaviour.AttackType, "isAttack");

        if (monobeHaviour.target)
            monobeHaviour.TransMoveToLocation();
        else
            monobeHaviour.TransMoveToDirection(Vector2.left);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}
