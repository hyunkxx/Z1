using System;
using UnityEngine;


public class ItemEquipmentParam : ItemParam
{
    public int CharacterID;
}

public class ItemEquipment : ItemBase
{
    private int EquippedCharacterID;
    public bool IsEquipped
    {
        get
        {
            return EquippedCharacterID != -1 ? true : false;
        }
    }

    public override void Initialize(ItemDataAsset data)
    {
        base.Initialize(data);

        EquippedCharacterID = -1;
    }

    public override void Use(ItemParam InParam = null)
    {
        base.Use(InParam);

        if (InParam != null)
        {
            ItemEquipmentParam param = InParam as ItemEquipmentParam;
            if (param != null)
            {
                EquippedCharacterID = param.CharacterID;
            }
        }
    }
}
