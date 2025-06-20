using UnityEngine;

public class Effect_Hit00 : MonoBehaviour
{
    private Effect2D m_Effect2D;
    private PoolContainer m_PoolContatiner;

    private void Awake()
    {
        var gameRule = GameManager.Instance.GameMode.Rule;
        m_PoolContatiner = gameRule.PoolContainer;

        m_Effect2D = GetComponent<Effect2D>();
        m_Effect2D.OnAnimmationFinished += AnimationFinishCallback;
    }

    private void AnimationFinishCallback()
    {
        var pool = m_PoolContatiner.GetPool("Effect_Hit00");
        pool.ReturnObject(transform.parent.gameObject);
    }
}
