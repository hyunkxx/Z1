using System.Data;
using UnityEngine;


/**
 * ID
 * Unlocked
 * CurrentLevel
 * CurrentExp
 * JobType
 **/

public class CharacterStatsRecord : IDatabaseModel<CharacterStatsRecord>
{
    public int ID { get; private set; }
    public int CurrentLevel { get; private set; }
    public int CurrentExp { get; private set; }
    public int BaseDamage { get; private set; }
    public int BaseArmor { get; private set; }
    public int BaseMaxHealth { get; private set; }
    public ECharacterType CharacterType { get; private set; }
    public ECharacterTier CharacterTier { get; private set; }

    public void Serialize(IDbCommand command)
    {
        command.Parameters.Clear();

        DatabaseService Service = Database.Instance.Service;
        Service.AddParameter(command, "@CharacterID", ID);
        Service.AddParameter(command, "@CurrentLevel", CurrentLevel);
        Service.AddParameter(command, "@CurrentExp", CurrentExp);
        Service.AddParameter(command, "@BaseDamage", BaseDamage);
        Service.AddParameter(command, "@BaseArmor", BaseArmor);
        Service.AddParameter(command, "@BaseMaxHealth", BaseMaxHealth);
        Service.AddParameter(command, "@CharacterType", (int)CharacterType);
        Service.AddParameter(command, "@CharacterTier", (int)CharacterTier);
    }

    public int Deserialize(IDataRecord record, params object[] args)
    {
        ID = record.GetInt32(0);
        CurrentLevel = record.GetInt32(1);
        CurrentExp = record.GetInt32(2);
        BaseDamage = record.GetInt32(3);
        BaseArmor = record.GetInt32(4);
        BaseMaxHealth = record.GetInt32(5);
        CharacterType = (ECharacterType)record.GetInt32(6);
        CharacterTier = (ECharacterTier)record.GetInt32(7);

        return ID;
    }
}