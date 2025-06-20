using UnityEngine;


public enum EItemCategory
{
    Consumable,
    Equipment,
    Currency,
    Material,
    Misc,
    Max
}

public enum EEquipmentType
{
    Helmet,
    Weapon,
    Accessory,
    Max
}

public enum EInventoryResult
{
    Success,
    Fail,
    Remain,
    Error,
    None
}

public sealed class InventoryResult
{
    public ItemBase Item { get; private set; }
    public int Amount { get; private set; }
    public EInventoryResult Result { get; private set; }

    public InventoryResult()
    {
        Result = EInventoryResult.None;
        Item = null;
        Amount = 0;
    }

    public InventoryResult(EInventoryResult result, ItemBase item = null, int amount = 0)
    {
        Result = result;
        Item = item;
        Amount = amount;
    }

    public bool IsSuccess => Result == EInventoryResult.Success;
}