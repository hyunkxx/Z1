using UnityEngine;


[CreateAssetMenu(fileName = "ActionAnimationTrigger", menuName = "Scriptable Objects/Actions/ActionAnimationTrigger")]
public class ActionAnimationTrigger : BaseAction
{
    [SerializeField] protected string m_animationTrigger;

    protected override bool InternalExecuteAction(ICharacterQueryable InQueryable)
    {
        Character character = InQueryable.GetCharacter();
        if (!string.IsNullOrEmpty(m_animationTrigger))
        {
            character.Animator.SetTrigger(m_animationTrigger);
            return true;
        }
        else
        {
            return false;
        }
    }
}
