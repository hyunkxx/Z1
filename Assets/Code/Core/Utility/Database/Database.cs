using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;

using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using System.IO;


/* temp struct */
public class ItemData : IDatabaseModel
{
    public string name;
    public int id;
    public int data;

    public virtual void Initialize(IDataRecord record, params object[] args)
    {
        name = record.GetString(0);
        id = record.GetInt32(1);
        data = record.GetInt32(2);
    }
}

public class Database : Singleton<Database>
{
    private DatabaseService databaseService;

    [SerializeField]
    private string DatabasePath = "Database/GameDatabase.db";


    #region TestCode

    /* Test Data */
    public List<TestCharacterData> CharcterList = new List<TestCharacterData>();

    #endregion

    /* TABLE SAMPLE */
    private Dictionary<int, ItemData> itemTable = new Dictionary<int, ItemData>();
    public IReadOnlyDictionary<int, ItemData> ItemTable => itemTable;

    public void Awake()
    {
        string streamingDBPath = Path.Combine(Application.streamingAssetsPath, DatabasePath);
        string persistentDBPath = Path.Combine(Application.persistentDataPath, DatabasePath);
        Debug.Log(persistentDBPath);

        AssetDuplicator.CopyFile(streamingDBPath, persistentDBPath, OnCopyComplete);

        /* Test Data */
        CharcterList.Add(new TestCharacterData(1000, "Backsu", (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath($"Assets/Art/Character/1000/character_1000.png", typeof(Sprite))));
        CharcterList.Add(new TestCharacterData(1001, "Vampire", (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath($"Assets/Art/Character/1001/character_1001.png", typeof(Sprite))));
    }
    public void OnCopyComplete(bool bSuccessful)
    {
        Debug.Log(bSuccessful);

        string persistentPath = Path.Combine(Application.persistentDataPath, DatabasePath);
        string connectionPath = $"URI=file:{persistentPath}";
        databaseService = new DatabaseService(connectionPath);

        /* generate cache */
        List<ItemData> itemList = databaseService.GetDataClassList<ItemData>();
        foreach (ItemData item in itemList)
        {
            itemTable.Add(item.id, item);
        }
    }

    public void OnDestroy()
    {
        databaseService.Dispose();
    }
}

/*Test Class*/
[System.Serializable]
public struct TestCharacterData
{
    public int ID;
    public string Name;
    public Sprite sprite;

    public TestCharacterData(int _id, string _name, Sprite _sprite)
    {
        ID = _id;
        Name = _name;
        sprite = _sprite;
    }
}
