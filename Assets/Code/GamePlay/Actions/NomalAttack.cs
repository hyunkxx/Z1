using UnityEngine;


public class NomalAttack : AttackAction
{
    protected override void Awake()
    {
        attackDelay = 3f;
        baseAttackDelay = 3f;
    }

    protected override void Update()
    {
        attackDelay -= Time.deltaTime;
    }

    public override void ExcuteAction()
    {

    }
}
