using UnityEngine;


public abstract class Ability : Z1Behaviour
{
    protected GameObject m_instigator;
    protected GameObject m_Target;

    protected override void Awake()
    {
        enabled = false;
    }

    public virtual void Activate(GameObject instigator, GameObject target = null)
    {
        m_instigator = instigator;
        m_Target = target;

        enabled = true;
    }
}
