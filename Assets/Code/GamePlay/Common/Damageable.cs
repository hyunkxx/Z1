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
    EnemyAI
}

public sealed class Damageable : MonoBehaviour
{
    [SerializeField] private ETeam teamID = ETeam.EnemyAI;
    public ETeam TeamID => teamID;

    public event Action<DamageEvent> OnDamageTaken;

    public bool IsAlly(Damageable other)
    {
        return other.TeamID == teamID;
    }
    public bool IsEnemy(Damageable other)
    {
        return other.TeamID != teamID;
    }
    public void TakeDamage(DamageEvent customEvent)
    {
        OnDamageTaken?.Invoke(customEvent);
    }
}