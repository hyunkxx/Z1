using UnityEngine;

public class AttackAction : BaseAction
{
    [SerializeField] protected GameObject EffectPrefab;
    protected Effect2D effect2D;
    protected Transform effectTransform;

    public float attackDelay = 10f;
    public float baseAttackDelay = 10f;
    public float attackRange = 1f;
    public AnimationClip clip;

    protected override void Update()
    {
        attackDelay -= Time.deltaTime;
    }


    public void SetEffectTransform(Transform target)
    {
        effectTransform = target;
    }

    public override void ExcuteAction() 
    {
        if (EffectPrefab == null)
            return;

        GameObject obj = Instantiate(EffectPrefab);
        effect2D = obj.GetComponent<Effect2D>();
        obj.SetActive(true);

        if(effectTransform == null)
        {
            effect2D.ActivateEffect(gameObject);
        }
        else
        {
            effect2D.ActivateEffect(gameObject, effectTransform);
        }
    }
}