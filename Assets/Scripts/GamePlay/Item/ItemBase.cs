using System;
using UnityEngine;


public abstract class ItemParam { }
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

    public virtual void Initialize(ItemDataAsset dataAsset) { DataAsset = dataAsset; }
    public virtual void Use(ItemParam InParam = null) { }
}