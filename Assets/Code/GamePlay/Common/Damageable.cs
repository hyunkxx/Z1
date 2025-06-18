using System;
using UnityEngine;


 /* weapon data, buff, debuff, region */
public struct AttackInfo
{

}

public class DamageEvent
{
    public float damage;
    public GameObject instigator;

    public DamageEvent(/*AttackInfo attackInfo,*/ float damage, GameObject instigator) 
    {
        this.damage = damage;
        this.instigator = instigator;
    }
}

public enum ETeam
{
    None,
    Player,
    Neutral,
    EnemyAI
}

public sealed class Damageable : MonoBehaviour
{
    [SerializeField] private ETeam teamID = ETeam.EnemyAI;
    public ETeam TeamID => teamID;

    public event Action<DamageEvent> OnDamageTaken;

    public bool IsAlly(Damageable other)
    {
        if (teamID == ETeam.Neutral || other.TeamID == ETeam.Neutral)
            return true;

        return other.TeamID == teamID;
    }

    public bool IsEnemy(Damageable other)
    {
        return teamID != other.TeamID && teamID != ETeam.Neutral && other.TeamID != ETeam.Neutral;
    }

    public void TakeDamage(DamageEvent customEvent)
    {
        /* temp defence object pool 아직 수정안됨 */
        ObjectPool hitEffectPool = PoolManager.Instance.GetPool("Effect_Hit00");
        if(hitEffectPool)
        {
            GameObject hitEffect = hitEffectPool.GetObject(transform.position, Quaternion.identity);
        }

        ObjectPool pool = PoolManager.Instance.GetPool("DamageFont");
        GameObject instance = pool.GetObject(transform.position, Quaternion.identity);
        DamageFont font = instance.GetComponent<DamageFont>();
        font.Show(customEvent, Color.white);

        OnDamageTaken?.Invoke(customEvent);
    }
}