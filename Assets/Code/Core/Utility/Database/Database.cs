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

    /* TABLE SAMPLE */
    private Dictionary<int, ItemData> itemTable = new Dictionary<int, ItemData>();
    public IReadOnlyDictionary<int, ItemData> ItemTable => itemTable;

    public void Awake()
    {
        string streamingDBPath = Path.Combine(Application.streamingAssetsPath, DatabasePath);
        string persistentDBPath = Path.Combine(Application.persistentDataPath, DatabasePath);
        Debug.Log(persistentDBPath);

        AssetDuplicator.CopyFile(streamingDBPath, persistentDBPath, OnCopyComplete);
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
