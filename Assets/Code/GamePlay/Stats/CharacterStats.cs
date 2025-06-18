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
    AttackDamage,        // �������ݷ�
    AttackSpeed,         // ���� �ӵ�
    Armor,               // ����
    MaxHealth,           // �ִ�ü��
    CurHealth,           // ����ü��
    CooldownReduction,   // ��Ÿ�� ����
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
        /* ������ ���� ó�� */
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

        /* �⺻ ������ ���� ���� ����ؼ� ���� */
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