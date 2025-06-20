using UnityEngine;
using UnityEngine.Animations;

public class LinkedSMB<TMonoBehaviour> : StateMachineBehaviour where TMonoBehaviour : MonoBehaviour
{
    protected TMonoBehaviour monobeHaviour;
    
    public static void Initialize(Animator _animator, TMonoBehaviour _monoBehaviour)
    {
        LinkedSMB<TMonoBehaviour>[] linkedSMB = _animator.GetBehaviours<LinkedSMB<TMonoBehaviour>>();
            
        for(int i = 0; i < linkedSMB.Length; ++i)
        {
            linkedSMB[i].InternalInitialize(_animator, _monoBehaviour);
        }
    }
        
    private void InternalInitialize(Animator _animator, TMonoBehaviour _monoBehaviour)
    {
        monobeHaviour = _monoBehaviour;
        //OnStart(animator)
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
    }
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex, controller);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.gameObject.activeSelf) return;
       
        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        if (!animator.gameObject.activeSelf) return;

        base.OnStateUpdate(animator, stateInfo, layerIndex, controller);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex, AnimatorControllerPlayable controller)
    {
        base.OnStateExit(animator, stateInfo, layerIndex, controller);
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash)
    {
        base.OnStateMachineEnter(animator, stateMachinePathHash);
    }

    public override void OnStateMachineEnter(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
    {
        base.OnStateMachineEnter(animator, stateMachinePathHash, controller);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash)
    {
        base.OnStateMachineExit(animator, stateMachinePathHash);
    }

    public override void OnStateMachineExit(Animator animator, int stateMachinePathHash, AnimatorControllerPlayable controller)
    {
        base.OnStateMachineExit(animator, stateMachinePathHash, controller);
    }
}
