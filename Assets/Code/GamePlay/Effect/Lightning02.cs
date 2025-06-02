//using System.Collections;
//using UnityEngine;

//public class Lightning02 : Action_SpawnEffect
//{
//    Collider2D col;
//    LineController lineController;

//    protected override void Awake()
//    {
//        base.Awake();
//        attackDelay = 10f;
//        BaseAttackDelay = attackDelay;
//    }

//    protected override void Start()
//    {
//        base.Start();
//    }
//    protected override void Update()
//    {
//        base.Update();
//        attackDelay -= Time.deltaTime;
//    }

//    public override void ExcuteAction()
//    {
//        // Instantiate Effect
//        base.ExcuteAction();
//        lineController = effect2D.gameObject.GetComponentInChildren<LineController>();
//        col = effect2D.gameObject.GetComponentInChildren<BoxCollider2D>();
//        // 스킬 로직
//        StartCoroutine(OnHit());
//    }

//    private IEnumerator OnHit()
//    {
//        int attackCount = 10;
//        CharacterStats stats = gameObject.GetComponent<CharacterStats>();
//        col.enabled = true;

//        while (attackCount > 0)
//        {
//            for (int i = 0; i < lineController.targetArray.Count; ++i)
//            {
//                Damageable owner = gameObject.GetComponent<Damageable>();
//                Damageable other = lineController.targetArray[i].gameObject.GetComponent<Damageable>();
//                if (owner && other)
//                {
//                    if (owner.IsEnemy(other))
//                    {
//                        DamageEvent damageEvent = new DamageEvent(stats.Damage, gameObject);
//                        other.TakeDamage(damageEvent);
//                    }
//                }
//            }
//            attackCount--;
//            yield return new WaitForSeconds(1f);
//        }

//        Destroy(effect2D.gameObject);
//    }
//}
