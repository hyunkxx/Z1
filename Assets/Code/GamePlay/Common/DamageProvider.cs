using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct DamageProviderParam
{
    [NonSerialized] public GameObject owner;
    [NonSerialized] public CharacterStats characterStats;

    [SerializeField] public bool shouldDisableOnTrigger;

    [SerializeField] public float duration;
    [SerializeField] public float periodicInterval;

    public DamageProviderParam(GameObject owner, CharacterStats stats, float duration, bool shouldDisableOnTrigger = false, float periodicInterval = 0f)
    {
        this.owner = owner;
        this.characterStats = stats;

        this.duration = duration;
        this.periodicInterval = periodicInterval;
        this.shouldDisableOnTrigger = shouldDisableOnTrigger;
    }
    public DamageProviderParam(DamageProviderParam param)
    {
        this.owner = param.owner;
        this.characterStats = param.characterStats;

        this.duration = param.duration;
        this.periodicInterval = param.periodicInterval;
        this.shouldDisableOnTrigger = param.shouldDisableOnTrigger;
    }
}

public class DamageProvider : MonoBehaviour
{
    [SerializeField] private int targetCount;
    [SerializeField] private DamageProviderParam providerData;

    private bool enableTrigger = false;
    Collider2D[] colls;

    private void Awake()
    {
        colls = GetComponents<Collider2D>();
        Debug.Assert(colls != null, "Effect2D has no assigned Collider.");

        foreach (Collider2D coll in colls)
        {
            coll.enabled = false;
            coll.isTrigger = true;
        }
    }

    protected void Activate()
    {
        ResetTrigger();

        StartCoroutine(CoroutineLifeTime());
        if(providerData.periodicInterval > 0f)
        {
            StartCoroutine(CoroutinePerodicActivate());
        }
    }
    public void ActivateProvider(GameObject owner, CharacterStats characterStats)
    {
        providerData.owner = owner;
        providerData.characterStats = characterStats;

        Activate();
    }

    private IEnumerator CoroutineLifeTime()
    {
        float elapsed = 0f;
        while(elapsed < providerData.duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        StopAllCoroutines();
        Destroy(gameObject);
    }
    private IEnumerator CoroutinePerodicActivate()
    {
        float elapsed = 0f;
        while(true)
        {
            yield return null;

            if (elapsed >= providerData.periodicInterval)
            {
                elapsed = 0f;
                ResetTrigger();
            }
            elapsed += Time.deltaTime;
        }
    }

    /* temp */
    private IEnumerator CoroutineResetTrigger(Collider2D[] Incolls)
    {
        foreach (Collider2D coll in Incolls)
        {
            coll.enabled = false;
            coll.isTrigger = true;
            yield return new WaitForFixedUpdate();
            coll.enabled = true;
        }
    }

    public void ResetTrigger()
    {
        foreach (Collider2D coll in colls)
        {
            coll.enabled = false;
            coll.isTrigger = true;
            coll.enabled = true;
        }

        enableTrigger = true;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enableTrigger)
            return;

        Damageable owner = providerData.owner.GetComponent<Damageable>();
        Damageable other = collision.gameObject.GetComponent<Damageable>();
        if (owner && other)
        {
            if (owner.IsEnemy(other))
            {
                DamageEvent damageEvent = new DamageEvent(providerData.characterStats.Damage, providerData.owner);
                other.TakeDamage(damageEvent);
            }
        }
    }
}
