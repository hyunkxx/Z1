using System;
using System.Collections;
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
    [SerializeField] DamageProviderParam providerData;
    private bool enableTrigger = false;

    protected void Activate()
    {
        enableTrigger = true;
        StartCoroutine(CoroutineActivateTimer());
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

    private IEnumerator CoroutineActivateTimer()
    {
        float elapsed = 0f;
        while(elapsed < providerData.duration)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }

        StopAllCoroutines();
        gameObject.SetActive(false);
    }
    private IEnumerator CoroutinePerodicActivate()
    {
        float elapsed = 0f;
        while(true)
        {
            enableTrigger = false;
            elapsed += Time.deltaTime;

            if (elapsed >= providerData.periodicInterval)
            {
                elapsed = 0f;
                enableTrigger = true;
            }

            yield return null;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!enableTrigger)
            return;

        Damageable owner = providerData.owner.GetComponent<Damageable>();
        Damageable other = collision.gameObject.GetComponent<Damageable>();
        if(owner && other)
        {
            if(owner.IsEnemy(other))
            {
                Debug.Log($"take damage {collision.gameObject.name}");
                DamageEvent damageEvent = new DamageEvent(providerData.characterStats.Damage, providerData.owner);
                other.TakeDamage(damageEvent);
            }
        }
    }
}
