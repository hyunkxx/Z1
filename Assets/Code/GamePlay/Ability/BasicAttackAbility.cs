using UnityEngine;


public class BasicAttackAbility : Ability
{
    private Rigidbody2D m_rg2d;
    private DamageProvider m_provider;

    protected override void Awake()
    {
        enabled = false;

        m_rg2d = GetComponent<Rigidbody2D>();
        m_provider = GetComponentInChildren<DamageProvider>();
    }

    public override void Activate(GameObject instigator, GameObject target = null)
    {
        m_instigator = instigator;
        m_Target = target;

        transform.position = target ? target.transform.position : instigator.transform.position;

        Character character = instigator.GetComponent<Character>();
        m_provider.ActivateProvider(instigator, character.Stats);

        enabled = true;
    }
}
