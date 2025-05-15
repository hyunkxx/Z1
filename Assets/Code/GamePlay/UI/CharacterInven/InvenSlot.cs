using UnityEngine;
using UnityEngine.UI;

public class InvenSlot : MonoBehaviour
{
    public TestItemData itemData;
    public Image ItemImage;

    public Sprite GetSprite()
    {
        return itemData.sprite;
    }


    public void SetSlotInfo(TestItemData _itemData)
    {
        itemData = _itemData;
        ItemImage.sprite = (itemData == null) ? null : itemData.sprite;
    }
}
