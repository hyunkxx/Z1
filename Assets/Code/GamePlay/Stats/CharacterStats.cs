using System;
using UnityEngine;


public enum EStatType
{
    AttackDamage,
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

    float[] _BaseStats = new float[(int)EStatType.Max];
    float[] _BonusStats = new float[(int)EStatType.Max];

    public CharacterStats(int characterID)
    {
        var Service = Database.Instance.Service;
        CharacterStatsRecord stats = Service.MakeClassByID<CharacterStatsRecord>(characterID);

        _BaseStats[(int)EStatType.AttackDamage]  = (float)stats.BaseDamage + (stats.BaseDamage * 0.1f) * stats.CurrentLevel - 1;
        _BaseStats[(int)EStatType.Armor]         = (float)stats.BaseArmor + (stats.BaseArmor * 0.1f) * stats.CurrentLevel - 1;
        _BaseStats[(int)EStatType.MaxHealth]     = (float)stats.BaseMaxHealth + (stats.CurrentLevel * 10.0f);
        _BaseStats[(int)EStatType.CurHealth]    = _BaseStats[(int)EStatType.MaxHealth];
    }

    public float GetStat(EStatType type)
    {
        return _BaseStats[(int)type] + _BonusStats[(int)type];
    }
}