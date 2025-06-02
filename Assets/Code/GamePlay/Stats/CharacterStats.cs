using System;
using UnityEngine;


public enum EStatType
{
    AttackDamage,
    AttackSpeed,
    Armor,
    MaxHealth,
    CurHealth,
    Max
}

//public enum ESubStatType
//{
//    AttackDamage,
//    AttackSpeed,
//    Armor,
//    ArmorPenetration,
//    CriticalDamage,
//    CriticalRate,
//    MaxHealth,
//    Max
//}

// @hyun:todo
public class CharacterStats
{
    //temp
    public float Damage = 50.0f;

    public int CharacterID { get; private set; }
    public EJobType JobType { get; private set; }

    float[] _baseStats = new float[(int)EStatType.Max];
    float[] _bonusStats = new float[(int)EStatType.Max];

    public CharacterStats(int characterID)
    {
        var Service = Database.Instance.Service;
        CharacterStatsRecord stats = Service.MakeClassByID<CharacterStatsRecord>(characterID);

        CharacterID = stats.ID;
        JobType = stats.JobType;

        _baseStats[(int)EStatType.AttackDamage]  = (float)stats.BaseDamage + (stats.BaseDamage * 0.1f) * stats.CurrentLevel - 1;
        _baseStats[(int)EStatType.Armor]         = (float)stats.BaseArmor + (stats.BaseArmor * 0.1f) * stats.CurrentLevel - 1;
        _baseStats[(int)EStatType.MaxHealth]     = (float)stats.BaseMaxHealth + (stats.CurrentLevel * 10.0f);
        _baseStats[(int)EStatType.CurHealth]     = _baseStats[(int)EStatType.MaxHealth];

        switch (JobType)
        {
            case EJobType.HandGun:
                _baseStats[(int)EStatType.AttackSpeed] = 1.0f;
                break;
            case EJobType.AR:
                _baseStats[(int)EStatType.AttackSpeed] = 1.0f;
                break;
            case EJobType.SMG:
                _baseStats[(int)EStatType.AttackSpeed] = 1.0f;
                break;
        }
    }

    public float GetStat(EStatType type)
    {
        return _baseStats[(int)type] + _baseStats[(int)type];
    }
}