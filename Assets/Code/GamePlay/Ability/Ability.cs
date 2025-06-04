using UnityEngine;


public abstract class Ability : MonoBehaviour
{
    protected GameObject m_instigator;
    protected GameObject m_Target;

    private void Awake()
    {
        enabled = false;
    }

    public virtual void Activate(GameObject instigator, GameObject target = null)
    {
        m_instigator = instigator;
        m_Target = target;

        transform.position = instigator.transform.position;
        enabled = true;
    }
}
