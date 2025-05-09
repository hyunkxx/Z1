using UnityEngine;


public abstract class BaseAction : Z1Behaviour, IAction
{
    [SerializeField]
    protected string actionName;
    public string ActionName => actionName;

    public virtual void ExcuteAction() { }
}
