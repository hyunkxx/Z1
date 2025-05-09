using System.Collections;
using UnityEngine;

public class Lightning02 : AttackAction
{
    Collider2D collider2D;
    LineController lineController;

    protected override void Awake()
    {
        base.Awake();
        attackDelay = 10f;
        baseAttackDelay = 10f;
    }

    protected override void Update()
    {
        base.Update();
        attackDelay -= Time.deltaTime;
    }

    public override void ExcuteAction()
    {
        // Instantiate Effect
        base.ExcuteAction();
        lineController = effect2D.gameObject.GetComponentInChildren<LineController>();
        collider2D = effect2D.gameObject.GetComponentInChildren<BoxCollider2D>();
        // 스킬 로직
        StartCoroutine(OnHit());
    }

    private IEnumerator OnHit()
    {
        int attackCount = 10;
        CharacterStats stats = gameObject.GetComponent<CharacterStats>();
        collider2D.enabled = true;

        while (attackCount > 0)
        {
            for (int i = 0; i < lineController.targetArray.Count; ++i)
            {
                Damageable owner = gameObject.GetComponent<Damageable>();
                Damageable other = lineController.targetArray[i].gameObject.GetComponent<Damageable>();
                if (owner && other)
                {
                    if (owner.IsEnemy(other))
                    {
                        DamageEvent damageEvent = new DamageEvent(stats.Damage, gameObject);
                        other.TakeDamage(damageEvent);

                        Rigidbody2D rg2d = owner.GetComponent<Rigidbody2D>();
                        Vector3 dir = owner.transform.position - other.transform.position;
                        rg2d.AddForce(dir.normalized * 0.25f, ForceMode2D.Impulse);
                    }
                }
            }
            attackCount--;
            yield return new WaitForSeconds(1f);
        }

        Destroy(effect2D.gameObject);
    }
}
