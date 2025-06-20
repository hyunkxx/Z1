using UnityEngine;


public class BuffAbility : Ability
{
    [SerializeField] private float m_buffTime;
    private float m_elapsedTime = 0f;

    private SpriteRenderer m_renderer;
    private Animator m_animator;

    protected override void Awake()
    {
        m_renderer = GetComponent<SpriteRenderer>();
        m_animator = GetComponent<Animator>();

        m_renderer.enabled = false;
        m_animator.enabled = false;
        enabled = false;
    }

    public override void Activate(GameObject instigator, GameObject target = null)
    {
        m_instigator = instigator;
        m_Target = target;

        m_elapsedTime = 0f;
        m_renderer.enabled = true;
        m_animator.enabled = true;
        enabled = true;

        transform.SetParent(target.transform);
        transform.position = target.transform.position;
        transform.localPosition = Vector3.zero;
    }

    public virtual void AddBuff()
    {
        /* apply stat bonus */
    }

    public virtual void RemoveBuff()
    {
        /* remove stat bonus */

        Destroy(gameObject);
    }

    protected override void Update()
    {
        m_elapsedTime += Time.deltaTime;
        if(m_elapsedTime >= m_buffTime)
        {
            RemoveBuff();
        }
    }
}