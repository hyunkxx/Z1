using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//@hyun_todo : damage type {region/tick/single/multi/}
public enum ELifeCycle
{
    Owner,
    Self
}

public sealed class DamageProvider : MonoBehaviour
{
    [SerializeField]
    public ELifeCycle lifeCycle;

    [SerializeField, ShowIf("lifeCycle", ELifeCycle.Self)]
    public float lifeTime;

    [SerializeField] public bool shouldDestroyOnTrigger;
    [SerializeField] public float periodicInterval;

    [NonSerialized] private GameObject owner;
    [NonSerialized] private CharacterStats characterStats;

    private Collider2D coll;
    private bool enableTrigger = false;

    public GameObject Owner => owner;

    private void Awake()
    {
        enabled = false;
        coll = GetComponent<Collider2D>();

        coll.enabled = false;
        coll.isTrigger = true;
    }

    public void ActivateProvider(GameObject owner, CharacterStats characterStats)
    {
        enabled = true;

        this.owner = owner;
        this.characterStats = characterStats;

        Activate();
    }

    private void Activate()
    {
        ResetTrigger();

        if(lifeCycle == ELifeCycle.Self)
        {
            StartCoroutine(CoroutineLifeTime());
        }

        if(periodicInterval > 0f)
        {
            StartCoroutine(CoroutinePerodicActivate());
        }
    }

    public void ResetTrigger()
    {
        coll.enabled = false;
        coll.isTrigger = true;
        coll.enabled = true;

        enableTrigger = true;
    }

    private IEnumerator CoroutineLifeTime()
    {
        float elapsed = 0f;
        while(elapsed < lifeTime)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        StopAllCoroutines();

        if (shouldDestroyOnTrigger)
            Destroy(transform.root.gameObject);
    }

    private IEnumerator CoroutinePerodicActivate()
    {
        float elapsed = 0f;
        while(true)
        {
            yield return null;

            if (elapsed >= periodicInterval)
            {
                elapsed = 0f;
                ResetTrigger();
            }
            elapsed += Time.deltaTime;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enableTrigger)
            return;

        if (this.owner == null)
            return;

        Damageable owner = this.owner.GetComponent<Damageable>();
        Damageable other = collision.gameObject.GetComponent<Damageable>();
        if (owner && other)
        {
            if (owner.IsEnemy(other))
            {
                //@hyun_temp
                DamageEvent damageEvent = new DamageEvent(50f, this.owner);

                //DamageEvent damageEvent = new DamageEvent(providerData.characterStats.Damage, providerData.owner);
                other.TakeDamage(damageEvent);

                if (shouldDestroyOnTrigger)
                    Destroy(transform.root.gameObject);
            }
        }
    }
}
