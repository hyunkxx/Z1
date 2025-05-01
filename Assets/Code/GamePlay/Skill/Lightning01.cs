using UnityEngine;

public class Lightning01 : Skill
{
    public GameObject EffectPrefab;

    private void Awake()
    {
        attackDelay = 10f;
        baseAttackDelay = 10f;
    }

    private void Update()
    {
        attackDelay -= Time.deltaTime;
    }

    public override void Action()
    {
        GameObject obj = Instantiate(EffectPrefab);
        obj.transform.position = Vector3.zero;
    }
}
