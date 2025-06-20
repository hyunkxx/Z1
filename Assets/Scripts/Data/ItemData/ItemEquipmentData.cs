using System;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct StatModifier
{
    public EStatType statType;
    public int value;
}

[CreateAssetMenu(fileName = "ItemEquipmentData", menuName = "Scriptable Objects/DataAsset/ItemEquipmentData")]
public sealed class ItemEquipmentData : ItemDataAsset
{
    [SerializeField] private EEquipmentType _equipmentType;
    public EEquipmentType EquipmentType => _equipmentType;

    [SerializeField]
    private List<StatModifier> Stats;
    public int StatModifierCount => Stats.Count;

    // @hyun:todo
    public void Apply(CharacterStats stat)
    {
        //Stats Apply
    }

    public void OnEnable()
    {
        Category = EItemCategory.Equipment;
    }

    public override Type GetItemClass()
    {
        return typeof(ItemEquipment);
    }
}
