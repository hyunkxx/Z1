using System;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;

using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;


public interface IDatabaseModel
{
    public void Initialize(IDataRecord record, params object[] args);
}

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
    private SQLiteDatabaseService sqliteService;

    /* TABLE SAMPLE */
    private Dictionary<int, ItemData> itemTable = new Dictionary<int, ItemData>();
    public IReadOnlyDictionary<int, ItemData> ItemTable => itemTable;

    public void Awake()
    {
        sqliteService = new SQLiteDatabaseService();
        
        List<ItemData> itemList = sqliteService.GetDataClassList<ItemData>();
        foreach (ItemData item in itemList)
        {
            itemTable.Add(item.id, item);
        }
        foreach (var e in ItemTable)
        {
            Debug.Log($"{e.Key} {e.Value.data} {e.Value.name}");
        }
    }
    public void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            List<ItemData> itemList = sqliteService.GetDataClassList<ItemData>();
            foreach (ItemData item in itemList)
            {
                Debug.Log($"{item.id} {item.data} {item.name}");
            }
        }
    }
    public void OnDestroy()
    {
        sqliteService.Dispose();
    }
}
