using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;


public readonly struct SlotIdentity
{
    public Inventory Inventory { get; }
    public Vector2Int SlotIndex { get; }
    public EItemCategory Category { get; }

    public SlotIdentity(Inventory outer, Vector2Int index, EItemCategory category)
    {
        Inventory = outer;
        SlotIndex = index;
        Category = category;
    }

    public int ConvertSlotID()
    {
        return (int)Category * (Inventory._volume.x * Inventory._volume.y) + (SlotIndex.y * Inventory._volume.x + SlotIndex.x);
    }
}

public sealed class ItemSlot : IDatabaseModel<ItemSlot>
{
    public ItemBase Item;
    public SlotIdentity Identity { get; private set; }
    public int StackCount { get; private set; }

    public event Action<int> OnChangedStackCount;

    public bool IsValid
    {
        get
        {
            return Item != null;
        }
    }
    public bool IsEmpty
    {
        get
        {
            return Item == null;
        }
    }
    public bool IsFull
    {
        get
        {
            return !IsEmpty && StackCount >= Item.DataAsset.StackLimit;
        }
    }

    public void Initialize(SlotIdentity identity)
    {
        Identity = identity;
    }

    //public void MarkDirty()
    //{
    //    Identity.Inventory.AddUpdatedSlot(this);
    //    OnChangedStackCount(StackCount);
    //}

    public void Swap(ItemSlot rhs)
    {
        ItemBase tempItem = rhs.Item;
        rhs.Item = Item;
        Item = tempItem;

        int stackCount = rhs.StackCount;
        rhs.StackCount = StackCount;
        StackCount = stackCount;

        Identity.Inventory.AddUpdatedSlot(this);

        OnChangedStackCount(StackCount);
        rhs?.OnChangedStackCount(rhs.StackCount);
    }
    public bool CanAddItem(ItemBase item)
    {
        if (IsEmpty)
            return true;
        else if (Item.DataAsset.ID == item.DataAsset.ID && !IsFull)
            return true;

        return false;
    }

    public void SetStackCount(int value)
    {
        if (StackCount + value == StackCount)
            return;

        if (Item.DataAsset.StackLimit < value)
        {
            value = Item.DataAsset.StackLimit;
        }

        StackCount = value;
        Identity.Inventory.AddUpdatedSlot(this);
        OnChangedStackCount?.Invoke(StackCount);
    }
    public int IncrementStack(int amount)
    {
        if (!Item.DataAsset.IsAllowStack)
            return amount;

        if (IsFull)
            return amount;

        StackCount += amount;
        if (StackCount > Item.DataAsset.StackLimit)
        {
            amount = StackCount - Item.DataAsset.StackLimit;
            StackCount = Item.DataAsset.StackLimit;
        }
        else
        {
            amount = 0;
        }

        Identity.Inventory.AddUpdatedSlot(this);
        OnChangedStackCount?.Invoke(StackCount);
        return amount;
    }
    public int DecrementStack(int amount)
    {
        int reduce;
        if (StackCount < amount)
        {
            reduce = StackCount;
            StackCount = 0;
            Item = null;
        }
        else
        {
            StackCount -= amount;
            reduce = amount;
        }

        Identity.Inventory.AddUpdatedSlot(this);
        OnChangedStackCount?.Invoke(StackCount);
        return reduce;
    }

    public void Serialize(IDbCommand command)
    {
        command.Parameters.Clear();

        var param = command.CreateParameter();
        param.ParameterName = "@ID";
        param.Value = Identity.ConvertSlotID();
        command.Parameters.Add(param);

        var paramCategory = command.CreateParameter();
        paramCategory.ParameterName = "@Category";
        paramCategory.Value = (int)Identity.Category;
        command.Parameters.Add(paramCategory);

        var paramID = command.CreateParameter();
        paramID.ParameterName = "@ItemID";
        paramID.Value = IsValid ? Item.DataAsset.ID : -1;
        command.Parameters.Add(paramID);

        var paramQuantity = command.CreateParameter();
        paramQuantity.ParameterName = "@Quantity";
        paramQuantity.Value = IsValid ? StackCount : 0;
        command.Parameters.Add(paramQuantity);
    }
    public int Deserialize(IDataRecord record, params object[] args)
    {
        int tableID = record.GetInt32(0);
        int ItemID = record.GetInt32(2);
        int Quantity = record.GetInt32(3);

        if (ItemID == -1)
        {
            Item = null;
            StackCount = 0;
        }
        else
        {
            Item = ItemFactory.CreateInstance(Database.Instance.FindItemAsset(ItemID));
            StackCount = Quantity;
        }

        return tableID;
    }
}