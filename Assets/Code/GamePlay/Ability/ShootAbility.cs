using UnityEngine;


[RequireComponent(typeof(Rigidbody2D))]
public class ShootAbility : Ability
{
    [SerializeField] 
    private ProjectileProperty m_projectileProperty;
    private Rigidbody2D m_rg2d;
    private DamageProvider m_provider;

    protected override void Awake()
    {
        m_rg2d = GetComponent<Rigidbody2D>();
        m_rg2d.gravityScale = 0f;
        m_provider = GetComponentInChildren<DamageProvider>();

        enabled = false;
    }

    public override void Activate(GameObject instigator, GameObject target = null)
    {
        m_instigator = instigator;
        m_Target = target;

        Character character = instigator.GetComponent<Character>();
        transform.position = character.CharacterView.m_weaponEndSocket.position;
        //transform.rotation = character.CharacterView.m_weaponEndSocket.rotation;
        
        Vector2 direction = (m_Target.transform.position - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation= Quaternion.Euler(0f, 0f, angle);

        m_rg2d.linearVelocity = direction * m_projectileProperty.speed;
        m_provider.ActivateProvider(instigator, character.Stats);

        enabled = true;
    }

    protected override void Update()
    {
        m_projectileProperty.activateTime += Time.deltaTime;
        if (m_projectileProperty.lifeTime <= m_projectileProperty.activateTime)
        {
            Destroy(gameObject);
        }
    }
}
