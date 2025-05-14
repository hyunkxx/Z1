using UnityEngine;
using UnityEngine.UI;

public class CharacterEquipUI : UIBase
{
    public int CurCharacterID = 0;
    
    [SerializeField] private GameObject Contents;
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
            }
        }
    }

    void UnEquip()
    {
        // Cur Equip Item = Equip False;
        // SlotImage.sprite = null
    }

    void Equip(int _itemID)
    {
        UnEquip();
        // Change Character Equip ItemID = _itemID
        // inventory Item = Equip True
        // SlotImage.sprite = inventory Item Sprite
    }

}