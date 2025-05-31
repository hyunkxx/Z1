using System;
using UnityEngine;


public enum EStatType
{
    BLA1,
    BLA2,
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

    float[] _BaseStat = new float[(int)EStatType.Max];
    float[] _BonusStat = new float[(int)EStatType.Max];
    float[] _EquipStat = new float[(int)EStatType.Max];

    public float GetStat(EStatType type)
    {
        //return final stat
        float final = 0.0f;
        return final;
    }

    public void ApplyStats(CharacterStatsRecord record)
    {
        // initialize stats
    }
}