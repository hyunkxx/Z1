using UnityEngine;


public class DamageEffect2D : Effect2D
{
    [Header("HitEffect2D Property")]
    [SerializeField] 
    protected DamageProvider damageProvider;
    protected Collider2D[] colls;

    protected override void Awake()
    {
        base.Awake();

        colls = GetComponentsInChildren<Collider2D>();
        Debug.Assert(colls != null, "Effect2D has no assigned Collider.");
        
        foreach (Collider2D coll in colls)
        {
            coll.enabled = false;
            coll.isTrigger = true;
        }
    }
    protected override void OnDisable()
    {
        base.OnDisable();
    }
    /* call from animation event */
    public virtual void OnHit()
    {
        if (owner == null)
            return;

        Debug.Log("OnHit");
        foreach (Collider2D coll in colls)
            coll.enabled = true;

        CharacterStats stats = owner.GetComponent<CharacterStats>();
        damageProvider.ActivateProvider(owner, stats);
    }
    protected override void EffectDeactivated()
    {
        sprite.color = new Color(1f, 1f, 1f, 0f);
    }
}
