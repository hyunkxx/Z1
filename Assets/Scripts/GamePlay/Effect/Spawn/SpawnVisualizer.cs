using System;
using System.Collections;
using UnityEngine;


public class SpawnVisualizer : MonoBehaviour
{
    private float m_LifeCycle;
    private SpriteRenderer m_Sprite;
    private Effect2D m_Effect2D;

    private Action m_OnTrigger;

    private void Awake()
    {
        m_Effect2D = GetComponent<Effect2D>();
        m_Sprite = GetComponentInChildren<SpriteRenderer>();

        m_Effect2D.OnAnimmationFinished += onAnimationFinished;
    }

    private void OnDestroy()
    {
        if(m_Effect2D)
        {
            m_Effect2D.OnAnimmationFinished -= onAnimationFinished;
        }
    }

    public void Initialize(Vector3 location, Action onAction)
    {
        Transform parentTranform = transform.parent;
        parentTranform.position = location;
        parentTranform.rotation = Quaternion.Euler(0, 0, UnityEngine.Random.Range(0f, 360f));

        m_OnTrigger = onAction;
    }

    private void onAnimationFinished()
    {
        m_OnTrigger?.Invoke();
    }

    private void OnEnable()
    {
    }

    private void OnDisable()
    {
        m_OnTrigger = null;
    }
}
