using System;
using UnityEngine;


public sealed class ItemFactory
{
    public static ItemBase CreateInstance(ItemDataAsset data)
    {
        if (data == null)
            return null;

        ItemBase item = (ItemBase)Activator.CreateInstance(data.GetItemClass(), data);
        item.Initialize(data);

        return item;
    }
}

public abstract class ItemBase
{
    public ItemDataAsset DataAsset { get; protected set; }
    public int StackCount { get; private set; }

    public event Action<int> OnChangedStackCount;

    public virtual void Initialize(ItemDataAsset dataAsset) { StackCount = 1; }
    public virtual void Use() { }

    public void SetStackCount(int value)
    {
        StackCount = value;
        OnChangedStackCount?.Invoke(StackCount);
    }
    public bool IncrementStack(int amount, out int remain)
    {
        remain = 0;

        if (!DataAsset.IsAllowStack)
            return false;

        int value = StackCount + amount;
        if (value > DataAsset.StackLimit)
        {
            remain = value - DataAsset.StackLimit;
            StackCount = DataAsset.StackLimit;
        }
        else
        {
            StackCount += amount;
        }

        OnChangedStackCount?.Invoke(StackCount);
        return true;
    }
    public int DecrementStack(int amount)
    {
        int result = StackCount - amount;
        if (result < 0)
        {
            int reduce = StackCount;
            StackCount = 0;
            OnChangedStackCount?.Invoke(StackCount);
            return reduce;
        }
        else
        {
            StackCount -= amount;

            OnChangedStackCount?.Invoke(StackCount);
            return amount;
        }
    }

}