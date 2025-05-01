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

public class Damageable : MonoBehaviour
{
    public event Action<DamageEvent> OnDamageTaken;

    public void TakeDamage(DamageEvent customEvent)
    {
        OnDamageTaken?.Invoke(customEvent);
    }
}