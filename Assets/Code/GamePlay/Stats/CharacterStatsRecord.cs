using System.Data;
using UnityEngine;


/**
 * ID
 * Unlocked
 * CurrentLevel
 * CurrentExp
 * JobType
 **/
public enum EJobType
{
    None,
    HandGun,
    SMG,
    AR,
    Max
}

public class CharacterStatsRecord : IDatabaseModel<CharacterStatsRecord>
{
    public int ID { get; private set; }
    public int Unlocked { get; private set; }
    public int CurrentLevel { get; private set; }
    public int CurrentExp { get; private set; }
    public int BaseDamage { get; private set; }
    public int BaseArmor { get; private set; }
    public int BaseMaxHealth { get; private set; }
    public EJobType JobType { get; private set; }

    public void Serialize(IDbCommand command)
    {
        command.Parameters.Clear();

        DatabaseService Service = Database.Instance.Service;
        Service.AddParameter(command, "@CharacterID", ID);
        Service.AddParameter(command, "@Unlocked", Unlocked);
        Service.AddParameter(command, "@CurrentLevel", CurrentLevel);
        Service.AddParameter(command, "@CurrentExp", CurrentExp);
        Service.AddParameter(command, "@BaseDamage", BaseDamage);
        Service.AddParameter(command, "@BaseArmor", BaseArmor);
        Service.AddParameter(command, "@BaseMaxHealth", BaseMaxHealth);
        Service.AddParameter(command, "@JobType", (int)JobType);
    }

    public int Deserialize(IDataRecord record, params object[] args)
    {
        ID = record.GetInt32(0);
        Unlocked = record.GetInt32(1);
        CurrentLevel = record.GetInt32(2);
        CurrentExp = record.GetInt32(3);
        BaseDamage = record.GetInt32(4);
        BaseArmor = record.GetInt32(5);
        BaseMaxHealth = record.GetInt32(6);
        JobType = (EJobType)record.GetInt32(7);

        return ID;
    }
}