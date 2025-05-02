using System.Collections;
using UnityEngine;

public class Lightning01 : Skill
{
    public GameObject EffectPrefab;
    Effect2D effect2D;


    private void Awake()
    {
        attackDelay = 10f;
        baseAttackDelay = 10f;
        effect2D = EffectPrefab.GetComponentInChildren<Effect2D>();
    }

    private void Update()
    {
        attackDelay -= Time.deltaTime;
    }

    public override void Action()
    {
        // Instantiate Effect
        GameObject obj = Instantiate(EffectPrefab);
        obj.transform.position = Vector3.zero;
        // 스킬 로직

        // 데미지 처리
    }
}
    