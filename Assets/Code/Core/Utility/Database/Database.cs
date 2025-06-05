using System;
using System.IO;
using System.Data;
using System.Data.Common;
using System.Collections;
using System.Collections.Generic;
using Mono.Data.Sqlite;

using UnityEngine;
using static UnityEngine.InputSystem.LowLevel.InputStateHistory;
using AYellowpaper.SerializedCollections;
using Unity.VisualScripting;


public class Database : Singleton<Database>
{
    public DatabaseService Service { get; private set; }

    [SerializeField]
    private string DatabasePath = "Database/GameDatabase.db";

    #region TestCode
    /* Test Data */
    public Dictionary<int, TestCharacterData> TestCharcterList = new Dictionary<int, TestCharacterData>();
    public Dictionary<int, TestItemData> TestItemTable = new Dictionary<int, TestItemData>();
    public List<TestItemData> TestInvenList = new List<TestItemData>();
    #endregion

    /* TEMP */
    [SerializeField, SerializedDictionary("Key", "Character DataAsset")]
    private SerializedDictionary<int, CharacterDataAsset> _characterDataAssets;
    public int CharacterAssetCount => _characterDataAssets.Count;

    [SerializeField, SerializedDictionary("Key", "Item DataAsset")]
    private SerializedDictionary<int, ItemDataAsset> _itemDataAssets;
    public int ItemAssetCount => _itemDataAssets.Count;

    [SerializeField, SerializedDictionary("Key", "Defense Round DataAsset")]
    public SerializedDictionary<string, RoundAssetData> DefenseRoundAssetData;

    [SerializeField, SerializedDictionary("Key", "Defence Reward DataAsset")]
    public SerializedDictionary<string, DefenceRewardAssetData> DefenseRewardAssetDatas;

    /* INVENTORY */
    public Inventory Inventory { get; private set; }

    public CharacterDataAsset FindCharacterAsset(int ID) { return _characterDataAssets.GetValueOrDefault(ID); }
    public ItemDataAsset FindItemAsset(int ID) { return _itemDataAssets.GetValueOrDefault(ID); }

    private void PostInitialize(string connectionPath)
    {
        Service = new DatabaseService(connectionPath);
        Inventory = new Inventory(new Vector2Int(5, 5));

        //_characterStatsTable = Service.MakeDictionaryFromTable<CharacterStatsRecord>();
    }

    protected override void Awake()
    {
        base.Awake();

        string streamingDBPath = Path.Combine(Application.streamingAssetsPath, DatabasePath);
        string persistentDBPath = Path.Combine(Application.persistentDataPath, DatabasePath);
        Debug.Log(persistentDBPath);

        if (File.Exists(persistentDBPath))
        {
            BuildVersion streamingVersion = ReadBuildVersion(streamingDBPath);
            BuildVersion persistentVersion = ReadBuildVersion(persistentDBPath);

            if(streamingVersion < persistentVersion)
            {
                PostInitialize($"URI=file:{persistentDBPath}");
            }
            else
            {
                AssetDuplicator.CopyFile(streamingDBPath, persistentDBPath, OnCopyComplete);
            }
        }
        else
        {
            AssetDuplicator.CopyFile(streamingDBPath, persistentDBPath, OnCopyComplete);
        }
    }

    public void OnCopyComplete(bool bSuccessful)
    {
        string persistentPath = Path.Combine(Application.persistentDataPath, DatabasePath);
        string connectionPath = $"URI=file:{persistentPath}";
        PostInitialize(connectionPath);

        using (IDbCommand command = Service.Connection.CreateCommand())
        {
            BuildVersion version = new BuildVersion(1);

            string query = "INSERT OR REPLACE INTO BuildVersion (ID, Dirty, AppVersion) VALUES (@ID, @Dirty, @AppVersion);";
            command.CommandType = CommandType.Text;
            command.CommandText = query;

            version.Serialize(command);
            command.ExecuteNonQuery();
        }
    }

    public bool TableExists(string TableName)
    {
        return Service.TableExists(TableName);
    }

    public BuildVersion ReadBuildVersion(string dbPath)
    {
        using (var service = new DatabaseService($"URI=file:{dbPath}"))
        {
            using (IDbConnection connection = service.Connection)
            {
                if (!service.TableExists("BuildVersion"))
                {
                    string CreateQuery = $@"
                    CREATE TABLE IF NOT EXISTS BuildVersion (
                        ID INTEGER PRIMARY KEY,
                        Dirty INTEGER DEFAULT 0,
                        AppVersion TEXT DEFAULT '{Application.version}'
                    );";

                    using (IDbCommand command = service.Connection.CreateCommand())
                    {
                        command.CommandType = CommandType.Text;
                        command.CommandText = CreateQuery;
                        command.ExecuteNonQuery();
                    }

                    string query = "INSERT OR REPLACE INTO BuildVersion (ID, Dirty, AppVersion) VALUES (@ID, @Dirty, @AppVersion);";
                    using (var command = connection.CreateCommand())
                    {
                        BuildVersion version = new BuildVersion(0);

                        command.CommandType = CommandType.Text;
                        command.CommandText = query;

                        version.Serialize(command);
                        command.ExecuteNonQuery();
                    }
                }

                return service.MakeClassByID<BuildVersion>(0);
            }
        }
    }

    protected override void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Inventory.TryAddItem(0, 3);
        }

        if (Input.GetMouseButtonDown(1))
        {
            Inventory.SaveInventory();
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            Inventory.LoadInventory();
        }


        if (Input.GetKeyDown(KeyCode.P))
        {
            Inventory.TEMP();
        }
    }

    protected override void OnDestroy()
    {
        Service.Dispose();
    }
}

/*Test Class*/
[System.Serializable]
public class TestCharacterData
{
    public int ID;
    public string Name;
    public Dictionary<TestItemType, TestItemData> CharacterEquipData = new Dictionary<TestItemType, TestItemData>() { { TestItemType.Helmet, null}, { TestItemType.Weapon, null} };
    public Sprite sprite;

    public TestCharacterData(int _id, string _name, Sprite _sprite)
    {
        ID = _id;
        Name = _name;
        sprite = _sprite;
    }
}

[System.Serializable]
public class TestItemData
{
    public int ID;
    public string Name;
    public bool isEquip = false;
    public TestItemType ItemType = TestItemType.None;
    public Sprite sprite;

    public TestItemData(int _id, string _name, TestItemType _itemType, Sprite _sprite)
    {
        ID = _id;
        Name = _name;
        ItemType = _itemType;
        sprite = _sprite;
    }
}

public enum TestItemType
{
    Helmet = 0,
    Weapon = 1,
    Use,
    Other,
    None,
}