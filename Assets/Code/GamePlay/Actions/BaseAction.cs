using UnityEngine;


public abstract class BaseAction : Z1Behaviour, IAction
{
    [SerializeField]
    protected string actionName;
    public string ActionName => actionName;

    public float attackDelay = 10f;
    public float baseAttackDelay = 10f;

    public virtual void ExcuteAction() { }
}
