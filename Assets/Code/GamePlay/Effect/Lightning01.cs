using System.Collections;
using UnityEngine;


public class Lightning01 : AttackAction
{
    protected override void Awake()
    {
        attackDelay = 10f;
        baseAttackDelay = 10f;
        //effect2D = EffectPrefab.GetComponentInChildren<Effect2D>();

        Debug.Log(effect2D);
    }

    protected override void Update()
    {
        attackDelay -= Time.deltaTime;
    }

    public override void ExcuteAction()
    {
        // Instantiate Effect
        base.ExcuteAction();
        // 스킬 로직
        
        // 데미지 처리
    }
}