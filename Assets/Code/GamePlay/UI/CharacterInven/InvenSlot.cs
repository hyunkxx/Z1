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


    public void SetSlotInfo(int _index)
    {
        itemData.ID = Database.Instance.TestInvenList[_index].ID;
        itemData.Name = Database.Instance.TestInvenList[_index].Name;
        itemData.sprite = Database.Instance.TestInvenList[_index].sprite;

        ItemImage.sprite = itemData.sprite;
    }
}
