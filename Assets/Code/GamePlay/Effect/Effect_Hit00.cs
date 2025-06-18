using UnityEngine;

public class Effect_Hit00 : MonoBehaviour
{
    private Effect2D m_Effect2D;
    private PoolContainer m_PoolContatiner;

    private void Awake()
    {
        m_PoolContatiner = PoolManager.Instance.GetComponent<PoolContainer>();

        m_Effect2D = GetComponent<Effect2D>();
        m_Effect2D.OnAnimmationFinished += AnimationFinishCallback;
    }

    private void AnimationFinishCallback()
    {
        var pool = m_PoolContatiner.GetPool("Effect_Hit00");
        pool.ReturnObject(transform.parent.gameObject);
    }
}
