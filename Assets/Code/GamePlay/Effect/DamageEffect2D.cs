using UnityEngine;


public class DamageEffect2D : Effect2D
{
    [Header("HitEffect2D Property")]
    [SerializeField] 
    protected DamageProvider damageProvider;

    protected override void Awake()
    {
        base.Awake();
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

        damageProvider.ResetTrigger();
        Character character = owner.GetComponent<Character>();
        CharacterStats stats = character.Stats;
        damageProvider.ActivateProvider(owner, stats);
    }
    protected override void EffectDeactivated()
    {
        sprite.color = new Color(1f, 1f, 1f, 0f);
    }
}
