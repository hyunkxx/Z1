using System;
using UnityEngine;


public enum ECharacterTier
{
    Normal,
    Elite,
    Boss,
    Max
}

public enum ECharacterType
{
    None,
    HandGun,
    SMG,
    AR,
    Max
}

public enum EStatType
{
    AttackDamage,        // 물리공격력
    AttackSpeed,         // 공격 속도
    Armor,               // 방어력
    MaxHealth,           // 최대체력
    CurHealth,           // 현재체력
    CooldownReduction,   // 쿨타임 감소
    Max
}

public abstract class ICharacterStatsBase
{
    public int CharacterID { get; protected set; }
    public ECharacterType CharacterType { get; protected set; }
    protected CharacterStatsRecord _statsRecord;

    protected float[] _baseStats = new float[(int)EStatType.Max];
    protected float[] _bonusStats = new float[(int)EStatType.Max];

    public float GetStat(EStatType type) { return _baseStats[(int)type] + _bonusStats[(int)type]; }
    public void HealthReset() { _baseStats[(int)EStatType.CurHealth] = _baseStats[(int)EStatType.MaxHealth]; }
    public void ApplyDamage(DamageEvent info)
    { 
        /* 데미지 공식 처리 */
        _baseStats[(int)EStatType.CurHealth] = Mathf.Max(0f, _baseStats[(int)EStatType.CurHealth] - info.damage);
    }
}

public class CharacterStats : ICharacterStatsBase
{
    public CharacterStats(int characterID)
    {
        var Service = Database.Instance.Service;
        _statsRecord = Service.MakeClassByID<CharacterStatsRecord>(characterID);

        CharacterID = _statsRecord.ID;
        CharacterType = _statsRecord.CharacterType;

        /* 기본 스텟은 레벨 별로 비례해서 증가 */
        _baseStats[(int)EStatType.AttackDamage] = (float)_statsRecord.BaseDamage + (_statsRecord.BaseDamage * 0.1f) * (_statsRecord.CurrentLevel - 1);
        _baseStats[(int)EStatType.Armor] = (float)_statsRecord.BaseArmor + (_statsRecord.BaseArmor * 0.1f) * (_statsRecord.CurrentLevel - 1);
        _baseStats[(int)EStatType.MaxHealth] = (float)_statsRecord.BaseMaxHealth + (_statsRecord.CurrentLevel - 1) * 10.0f;
        _baseStats[(int)EStatType.CurHealth] = _baseStats[(int)EStatType.MaxHealth];

        switch (CharacterType)
        {
            case ECharacterType.HandGun:
                _baseStats[(int)EStatType.AttackSpeed] = 1.0f;
                break;
            case ECharacterType.AR:
                _baseStats[(int)EStatType.AttackSpeed] = 1.0f;
                break;
            case ECharacterType.SMG:
                _baseStats[(int)EStatType.AttackSpeed] = 1.0f;
                break;
        }
    }
}