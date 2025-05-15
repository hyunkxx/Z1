using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterEquipUI : UIBase
{
    private int CurCharacterID = 1000;
    [SerializeField] private ItemInventoryUI ItemIvenUI;

    [SerializeField] private GameObject Contents;
    private TestItemData PrevInvenItem = null;
    private GameObject CurSlot;
    private TestItemType CurSlotItemType = TestItemType.None;
    private GameObject[] Slots;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int ContentsSize = Contents.transform.childCount;
        Slots = new GameObject[ContentsSize];
        for (int i = 0; i < ContentsSize; ++i)
        {
            Slots[i] = Contents.transform.GetChild(i).gameObject;
            Contents.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(OnClickCharacterSlot);
        }
    }

    public void UpdateSlotUI(int _ID)
    {
        CurCharacterID = _ID;
        Dictionary<TestItemType, TestItemData> characterEquipData = Database.Instance.TestCharcterList[_ID].CharacterEquipData;
        for (int i = 0; i < characterEquipData.Count; ++i)
        {
            if (characterEquipData[(TestItemType)i] == null)
                Slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
            else
                Slots[i].transform.GetChild(0).GetComponent<Image>().sprite = characterEquipData[(TestItemType)i].sprite;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClickCharacterSlot()
    {
        for (int i = 0; i < Slots.Length; ++i)
        {
            if (GetButtonName() == Slots[i].name)
            {
                CurSlot = Slots[i];
                CurSlotItemType = (TestItemType)i;

                GetGameObject((int)GameObjects.ItemInven_Panel).SetActive(true);
                ItemIvenUI.SetSlotInfo(CurSlotItemType);
            }
        }
    }

    void UnEquip()
    {
        PrevInvenItem.isEquip = false;
        Database.Instance.TestCharcterList[CurCharacterID].CharacterEquipData[CurSlotItemType] = null;
    }

    public void Equip(TestItemData _item)
    {
        // 다른 캐릭이 장착중인지 현재 장착중인지 체크해야함.
        if (PrevInvenItem == null)
            PrevInvenItem = _item;
        else
            UnEquip();

        Database.Instance.TestCharcterList[CurCharacterID].CharacterEquipData[CurSlotItemType] = _item;
       _item.isEquip = true;
        CurSlot.transform.GetChild(0).GetComponent<Image>().sprite = _item.sprite;

        PrevInvenItem = _item;
    }

}