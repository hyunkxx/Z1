using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class ExplosionBulletAbility : Ability
{
    [SerializeField] float m_throwPower;
    
    /* ROOT COMP */
    private Rigidbody2D rg2d;
    private SpriteRenderer sprite;
    private DamageProvider damageProvider;

    /* MODEL COMP */
    private GameObject model;
    private Animator animator;
    private Effect2D effect2D;

    protected override void Awake() {}

    public override void Activate(GameObject instigator, GameObject target = null)
    {
        m_instigator = instigator;
        m_Target = target;
    }

    protected override void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        rg2d = GetComponent<Rigidbody2D>();
        damageProvider = GetComponentInChildren<DamageProvider>();

        rg2d.gravityScale = 0f;
        rg2d.linearVelocity = transform.right * m_throwPower;

        model = transform.Find("Model").gameObject;
        animator = model.GetComponent<Animator>();

        effect2D = model.GetComponent<Effect2D>();
        effect2D.OnAnimmationFinished += AnimationEndCallback;
        effect2D.OnAnimationEvent += AnimationEventCallback;

        model.SetActive(false);
    }

    protected override void FixedUpdate()
    {
        if (rg2d.linearVelocity.sqrMagnitude < 0.05f)
            ExplosionSetup();
    }

    private void ExplosionSetup()
    {
        rg2d.linearVelocity = Vector2.zero;

        sprite.enabled = false;
        model.SetActive(true);
        animator.transform.rotation = Quaternion.identity;
        enabled = false;
    }

    private void AnimationEventCallback(string param)
    {
        if(param == "Hit")
        {
            /* stat temp */
            damageProvider.ActivateProvider(m_instigator, null);
        }
    }

    private void AnimationEndCallback()
    {
        effect2D.OnAnimmationFinished -= AnimationEndCallback;
        effect2D.OnAnimationEvent -= AnimationEventCallback;

        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_instigator == collision.transform.root.gameObject)
            return;

        Damageable owner = m_instigator.GetComponent<Damageable>();
        Damageable other = collision.gameObject.GetComponent<Damageable>();
        if (owner == null || other == null || owner.IsAlly(other))
            return;

        ExplosionSetup();
    }
}
