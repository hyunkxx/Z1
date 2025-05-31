using System.Data;
using UnityEngine;


/* ummmmmmmmmmmmmmmmmmmmm */
public class CharacterStatsRecord : IDatabaseModel<CharacterStatsRecord>
{
    public int CharacterID { get; private set; }
    public int Unlocked { get; private set; }
    public int Level { get; private set; }
    public int STR { get; private set; }
    public int DEX { get; private set; }
    public int VIT { get; private set; }
    public int INT { get; private set; }
    public int LUK { get; private set; }

    public void Serialize(IDbCommand command)
    {
        command.Parameters.Clear();

        DatabaseService Service = Database.Instance.Service;
        Service.AddParameter(command, "@CharacterID", CharacterID);
        Service.AddParameter(command, "@Unlocked", Unlocked);
        Service.AddParameter(command, "@Level", Level);
        Service.AddParameter(command, "@STR", STR);
        Service.AddParameter(command, "@DEX", DEX);
        Service.AddParameter(command, "@VIT", VIT);
        Service.AddParameter(command, "@INT", INT);
        Service.AddParameter(command, "@LUK", LUK);
    }

    public int Deserialize(IDataRecord record, params object[] args)
    {
        CharacterID = record.GetInt32(0);
        Unlocked = record.GetInt32(1);
        Level = record.GetInt32(2);
        STR = record.GetInt32(3);
        DEX = record.GetInt32(4);
        VIT = record.GetInt32(5);
        INT = record.GetInt32(6);
        LUK = record.GetInt32(7);

        return CharacterID;
    }
}