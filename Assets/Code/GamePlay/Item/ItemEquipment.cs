using System;
using UnityEngine;


public class ItemEquipment : ItemBase
{
    private bool bEquipped;

    public override void Initialize(ItemDataAsset data)
    {
        base.Initialize(data);

        DataAsset = data;
        bEquipped = false;
    }

    public override void Use()
    {
        bEquipped = !bEquipped;
    }
}
