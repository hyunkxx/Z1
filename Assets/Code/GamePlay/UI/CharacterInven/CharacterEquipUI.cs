using UnityEngine;
using UnityEngine.UI;

public class CharacterEquipUI : UIBase
{
    public int CurCharacterID = 0;

    [SerializeField] private GameObject Contents;
    private int PrevInvenIndex = 0;
    private GameObject CurSlot;
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
                GetGameObject((int)GameObjects.ItemInven_Panel).SetActive(true);
                CurSlot = Slots[i];
            }
        }
    }

    void UnEquip()
    {
        // 이전 아이템 장착 해제
        Database.Instance.TestInvenList[PrevInvenIndex].isEquip = false;
    }

    public void Equip(int _itemID, int _invenIdex)
    {
        // Check Other Character 
        if (PrevInvenIndex == 0)
            PrevInvenIndex = _invenIdex;
        else
            UnEquip();

        // inventory Item = Equip True
        Database.Instance.TestInvenList[_invenIdex].isEquip = true;
        // SlotImage.sprite = inventory Item Sprite
        CurSlot.transform.GetChild(0).GetComponent<Image>().sprite = Database.Instance.TestInvenList[_invenIdex].sprite;

        PrevInvenIndex = _invenIdex;
    }

}