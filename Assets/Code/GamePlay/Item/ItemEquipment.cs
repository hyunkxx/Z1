using System;
using UnityEngine;


public class ItemEquipmentParam : ItemParam
{
    public int CharacterID;
}

public class ItemEquipment : ItemBase
{
    private int bEquippedCharacterID;
    public bool IsEquipped
    {
        get
        {
            return bEquippedCharacterID != -1 ? true : false;
        }
    }

    public override void Initialize(ItemDataAsset data)
    {
        base.Initialize(data);

        bEquippedCharacterID = -1;
    }

    public override void Use(ItemParam InParam = null)
    {
        base.Use(InParam);

        if (InParam != null)
        {
            ItemEquipmentParam param = InParam as ItemEquipmentParam;
            if (param != null)
            {
                bEquippedCharacterID = param.CharacterID;
            }
        }
    }
}
