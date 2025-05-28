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

/* temp struct */
public class ItemData : IDatabaseModel<ItemData>
{
    public string name { get; private set; }
    public int id { get; private set; }
    public int data { get; private set; }

    public virtual void Initialize(IDataRecord record, params object[] args)
    {
        name = record.GetString(0);
        id = record.GetInt32(1);
        data = record.GetInt32(2);
    }

    public ItemData Clone()
    {
        ItemData clone = new ItemData();
        clone.name = name;
        clone.id = id;
        clone.data = data;

        return clone;
    }
}

public class Database : Singleton<Database>
{
    private DatabaseService databaseService;

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

    /* TABLE SAMPLE */
    private Dictionary<int, ItemData> itemTable = new Dictionary<int, ItemData>();
    public IReadOnlyDictionary<int, ItemData> ItemTable => itemTable;

    public CharacterDataAsset FindCharacterAsset(int ID) { return _characterDataAssets.GetValueOrDefault(ID); }
    public ItemDataAsset FindItemAsset(int ID) { return _itemDataAssets.GetValueOrDefault(ID); }

    protected override void Awake()
    {
        base.Awake();

        string streamingDBPath = Path.Combine(Application.streamingAssetsPath, DatabasePath);
        string persistentDBPath = Path.Combine(Application.persistentDataPath, DatabasePath);
        Debug.Log(persistentDBPath);

        AssetDuplicator.CopyFile(streamingDBPath, persistentDBPath, OnCopyComplete);

        /* Test Data */
        TestCharcterList.Add(1000, new TestCharacterData(1000, "Backsu", (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath($"Assets/Art/Character/1000/character_1000.png", typeof(Sprite))));
        TestCharcterList.Add(1001, new TestCharacterData(1001, "Vampire", (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath($"Assets/Art/Character/1001/character_1001.png", typeof(Sprite))));
        TestItemTable.Add(100001, new TestItemData(100001, "TestWeapon01", TestItemType.Weapon, (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath($"Assets/Art/Weapon/AxeLong1.png", typeof(Sprite))));
        TestItemTable.Add(100002, new TestItemData(100002, "TestWeapon02", TestItemType.Weapon, (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath($"Assets/Art/Weapon/AxeNormal1.png", typeof(Sprite))));
        TestItemTable.Add(100003, new TestItemData(100003, "TestArmor01", TestItemType.None, (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath($"Assets/Art/Weapon/Normal_Armor1.png", typeof(Sprite))));
        TestItemTable.Add(100004, new TestItemData(100004, "TestArmor02", TestItemType.None, (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath($"Assets/Art/Weapon/Normal_Cloth1.png", typeof(Sprite))));
        TestItemTable.Add(100005, new TestItemData(100005, "TestHelmet01", TestItemType.Helmet, (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath($"Assets/Art/Weapon/Normal_Helmet1.png", typeof(Sprite))));
        TestItemTable.Add(100006, new TestItemData(100006, "TestHelmet02", TestItemType.Helmet, (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath($"Assets/Art/Weapon/Normal_Helmet2.png", typeof(Sprite))));
        TestInvenList.Add(TestItemTable[100001]);
        TestInvenList.Add(TestItemTable[100001]);
        TestInvenList.Add(TestItemTable[100002]);
        TestInvenList.Add(TestItemTable[100005]);
        TestInvenList.Add(TestItemTable[100006]);
    }

    public void OnCopyComplete(bool bSuccessful)
    {
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

    protected override void OnDestroy()
    {
        databaseService.Dispose();
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