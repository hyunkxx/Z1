using UnityEngine;


public class AttackAction : BaseAction
{
    [SerializeField] protected GameObject EffectPrefab;
    protected Effect2D effect2D;
    protected Transform effectTransform;

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

        Debug.Log($"Skill Action {gameObject.name}");
    }
}