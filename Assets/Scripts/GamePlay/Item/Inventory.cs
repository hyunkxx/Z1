using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


public sealed class Inventory
{
    public Dictionary<EItemCategory , ItemSlot[,]> _container = new Dictionary<EItemCategory, ItemSlot[,]>();
    public Vector2Int _volume { get; private set; }

    private List<ItemSlot> _updatedSlots = new List<ItemSlot>();
    public int UpdatedSlotCount => _updatedSlots.Count;

    public Inventory(Vector2Int volume)
    {
        _volume = volume;

        var DatabaseService = Database.Instance.Service;
        if (DatabaseService.TableExists(typeof(ItemSlot).Name))
        {
            LoadInventory();
        }
        else
        {
            const string CreateQuery = @"
            CREATE TABLE IF NOT EXISTS ItemSlot (
                ID INTEGER PRIMARY KEY,
                Category INTEGER,
                ItemID INTEGER,
                Quantity INTEGER
            );";

            using (IDbCommand command = DatabaseService.Connection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = CreateQuery;
                command.ExecuteNonQuery();
            }

            for (int category = 0; category < (int)EItemCategory.Max; ++category)
            {
                _container[(EItemCategory)category] = new ItemSlot[_volume.y, _volume.x];

                for (int y = 0; y < _volume.y; y++)
                {
                    for (int x = 0; x < _volume.x; x++)
                    {
                        _container[(EItemCategory)category][y, x] = new ItemSlot();

                        SlotIdentity identity = new SlotIdentity(this, new Vector2Int(x, y), (EItemCategory)category);
                        _container[(EItemCategory)category][y, x].Initialize(identity);
                    }
                }
            }

            SaveInventory();
        }
    }

    public void SaveInventory()
    {
        IDbConnection DBConnection = Database.Instance.Service.Connection;
        using(IDbTransaction transaction = DBConnection.BeginTransaction())
        {
            const string query = "INSERT OR REPLACE INTO ItemSlot (ID, Category, ItemID, Quantity) VALUES (@ID, @Category, @ItemID, @Quantity);";
            using (IDbCommand command = DBConnection.CreateCommand())
            {
                command.CommandType = CommandType.Text;
                command.CommandText = query;
                command.Transaction = transaction;

                for (int category = 0; category < (int)EItemCategory.Max; ++category)
                {
                    for (int y = 0; y < _volume.y; y++)
                    {
                        for (int x = 0; x < _volume.x; x++)
                        {
                            ItemSlot currentSlot = _container[(EItemCategory)category][y, x];

                            currentSlot.Serialize(command);
                            command.ExecuteNonQuery();
                        }
                    }
                }
            }

            transaction.Commit();
        }
    }
    public void LoadInventory()
    {
        List<ItemSlot> SlotDataRecords = Database.Instance.Service.MakeListFromTable<ItemSlot>();

        int index = 0;
        for (int category = 0; category < (int)EItemCategory.Max; ++category)
        {
            _container[(EItemCategory)category] = new ItemSlot[_volume.y, _volume.x];

            for (int y = 0; y < _volume.y; y++)
            {
                for (int x = 0; x < _volume.x; x++)
                {
                    _container[(EItemCategory)category][y, x] = SlotDataRecords[index++];

                    SlotIdentity identity = new SlotIdentity(this, new Vector2Int(x, y), (EItemCategory)category);
                    _container[(EItemCategory)category][y, x].Initialize(identity);
                }
            }
        }
    }
    public void AddUpdatedSlot(ItemSlot slot)
    {
        _updatedSlots.Add(slot);
    }
    public void SaveUpdatedSlots()
    {
        if (UpdatedSlotCount < 0)
            return;
    }

    public bool IsValidIndex(Vector2Int index)
    {
        return index.x >= 0 && index.x < _volume.x && index.y >= 0 && index.y >= _volume.y;
    }
    public ItemSlot FindFirstEmptySlot(EItemCategory category)
    {
        for (int y = 0; y < _volume.y; y++)
        {
            for (int x = 0; x < _volume.x; x++)
            {
                if (_container[category][y, x].IsEmpty)
                {
                    return _container[category][y, x];
                }
            }
        }

        return null;
    }
    public ItemSlot GetSlotByIndex(EItemCategory category, Vector2Int index)
    {
        return _container[category][index.y, index.x];
    }
    public ItemSlot[,] GetAllSlots(EItemCategory category)
    {
        return _container[(EItemCategory)category];
    }
    public int GetUsedSlotCount(EItemCategory category, List<ItemSlot> usedList = null)
    {
        int useCount = 0;
        for (int y = 0; y < _volume.y; y++)
        {
            for (int x = 0; x < _volume.x; x++)
            {
                if (!_container[(EItemCategory)category][y, x].IsEmpty)
                {
                    useCount++;

                    if(usedList != null)
                    {
                        usedList.Add(_container[(EItemCategory)category][y, x]);
                    }
                }
            }
        }

        return useCount;
    }

    public int TryAddItem(ItemSlot slot, int amount = 1)
    {
        if (!slot.IsEmpty)
            return amount;

        amount = slot.IncrementStack(amount);

        return amount;
    }
    public InventoryResult TryAddItem(int itemID, int amount = 1)
    {
        ItemDataAsset assetData = Database.Instance.FindItemAsset(itemID);
        if (assetData == null)
        {
            return new InventoryResult(EInventoryResult.Error, null, amount);
        }

        ItemBase item = ItemFactory.CreateInstance(assetData);
        if (item == null)
        {
            return new InventoryResult(EInventoryResult.Error, null, amount);
        }

        EItemCategory category = item.DataAsset.Category;
        for (int y = 0; y < _volume.y; y++)
        {
            for (int x = 0; x < _volume.x; x++)
            {
                if(_container[category][y, x].IsEmpty)
                {
                    _container[category][y, x].Item = item;
                    amount = _container[category][y, x].IncrementStack(amount);
                }
                else if(_container[category][y, x].CanAddItem(item))
                {
                    amount = _container[category][y, x].IncrementStack(amount);
                }

                if (amount <= 0)
                {
                    return new InventoryResult(EInventoryResult.Success);
                }
            }
        }

        return new InventoryResult(EInventoryResult.Remain, item, amount);
    }
    public InventoryResult TryAddItem(ItemBase item, int amount = 1)
    {
        EItemCategory category = item.DataAsset.Category;
        for (int y = 0; y < _volume.y; y++)
        {
            for (int x = 0; x < _volume.x; x++)
            {
                if (_container[category][y, x].IsEmpty)
                {
                    _container[category][y, x].Item = item;
                    amount = _container[category][y, x].IncrementStack(amount);
                }
                else if (_container[category][y, x].CanAddItem(item))
                {
                    amount = _container[category][y, x].IncrementStack(amount);
                }

                if (amount <= 0)
                {
                    return new InventoryResult(EInventoryResult.Success);
                }
            }
        }
        
        return new InventoryResult(EInventoryResult.Remain, item, amount);
    }

    public int TryRemoveItem(ItemSlot slot, int amount = 1)
    {
        if (slot.IsEmpty)
            return amount;

        amount = slot.DecrementStack(amount);

        return amount;
    }
    public InventoryResult TryRemoveItem(int itemID, int amount = 1)
    {
        ItemDataAsset assetData = Database.Instance.FindItemAsset(itemID);
        if (assetData == null)
        {
            return new InventoryResult(EInventoryResult.Error, null, amount);
        }

        EItemCategory category = assetData.Category;
        for (int y = 0; y < _volume.y; y++)
        {
            for (int x = 0; x < _volume.x; x++)
            {
                if (_container[category][y, x].IsEmpty)
                    continue;

                ItemBase item = _container[category][y, x].Item;
                if(item.DataAsset.ID == itemID)
                {
                    amount = _container[category][y, x].DecrementStack(amount);
                }

                if (amount <= 0)
                {
                    return new InventoryResult(EInventoryResult.Success);
                }
            }
        }
        
        return new InventoryResult(EInventoryResult.Remain, null, amount);
    }


    public void TEMP()
    {
        for (int category = 0; category < (int)EItemCategory.Max; ++category)
        {
            for (int y = 0; y < _volume.y; y++)
            {
                for (int x = 0; x < _volume.x; x++)
                {
                    Debug.Log(_container[(EItemCategory)category][y, x].Identity.ConvertSlotID());
                }
            }
        }
    }
}
