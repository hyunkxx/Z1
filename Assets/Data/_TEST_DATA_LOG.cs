using UnityEngine;
using System.Collections.Generic;

public class _TEST_DATA_LOG : MonoBehaviour
{
    void Start()
    {
        ItemDataAsset Asset = Database.Instance.FindItemAsset(0);
        ItemBase item = ItemFactory.CreateInstance(Asset);
        Debug.Log(item.DataAsset.IsAllowStack);
        Debug.Log(item.DataAsset.StackLimit);
        Debug.Log(item.GetType());
        ItemEquipmentData equipmentData = Asset as ItemEquipmentData;
        Debug.Log(equipmentData.GetType());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
