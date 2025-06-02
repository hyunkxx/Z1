using UnityEngine;


public class State_Idle : LinkedSMB<TempStateMachine>
{
    TargetingComponent _targetingComponent;
    BaseAction Action;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        _targetingComponent = monobeHaviour.TargetingComponent;
        Debug.Log($"Enter {monobeHaviour}");
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        Debug.Log(_targetingComponent.HasNearTarget());
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }
}