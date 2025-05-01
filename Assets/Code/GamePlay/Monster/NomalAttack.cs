using UnityEngine;

public class NomalAttack : Skill
{
    private void Awake()
    {
        attackDelay = 3f;
        baseAttackDelay = 3f;
    }

    void Start()
    {
        
    }

    private void Update()
    {
        attackDelay -= Time.deltaTime;
    }

    public override void Action()
    {
        Debug.Log("Noral Attack");
    }
}
