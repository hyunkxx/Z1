using System;
using UnityEngine;

[System.Serializable]
public class CharacterStats : MonoBehaviour
{
    [SerializeField] protected int curLevel = 1;
    [SerializeField] protected int maxLevel = 255;

    [SerializeField] protected float damage;
    [SerializeField] protected float damageModify;

    [SerializeField] protected float curHealth;
    [SerializeField] protected float maxHealth;

    [SerializeField] protected float curExp;
    [SerializeField] protected float maxExp;

    private event Action<int> OnLevelupEvent;
    private event Action<GameObject> OnDieEvent;

    public int CurLevel => curLevel;
    public int MaxLevel => maxLevel;
    public float CurHealth => curHealth;
    public float MaxHealth => maxHealth;
    public float CurExp => curExp;
    public float MaxExp => maxExp;

    public float Damage => damage;
    public float DamageModify => damageModify;

    public void Die()
    {
        curHealth = 0f;
        OnDieEvent?.Invoke(gameObject);
    }

    public void ReduceHealth(float value)
    {
        curHealth -= value;
        if (curHealth <= 0f)
        {
            curHealth = 0f;
            Die();
        }
    }
    public void AddHealth(float value)
    {
        curHealth += value;
        if (curHealth >= maxHealth)
        {
            curHealth = maxHealth;
        }
    }
    public void AddExp(float amount)
    {
        curExp += amount;
        while (curExp >= maxExp)
        {
            curExp -= maxExp;
            LevelUp();

            maxExp *= 1.25f;
        }

        curExp %= maxExp;
    }
    private void LevelUp()
    {
        curLevel++;
        OnLevelupEvent?.Invoke(curLevel);
    }
}
