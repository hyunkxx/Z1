using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class ItemInventoryUI : UIBase
{
    [SerializeField] private GameObject Contents;
    [SerializeField] private SelectItemUI selectItemUI;
    [SerializeField] private TextMeshProUGUI ItemInven_CurItemType_txt;
    private GameObject[] Slots;
    private Button[] TabButtons;

    private TestItemType CurItemType;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int ContentsSize = Contents.transform.childCount;

        Slots = new GameObject[ContentsSize];

        for (int i = 0; i < ContentsSize; ++i)
        {
            Slots[i] = Contents.transform.GetChild(i).gameObject;
            Contents.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(OnClickItemSlot);
        }

        GetButton((int)Buttons.ItemInven_Close_btn).onClick.AddListener(()=> { GetGameObject((int)GameObjects.ItemInven_Panel).SetActive(false); });
        GetButton((int)Buttons.ItemInven_Sort_btn).onClick.AddListener(SortItem);

        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void OnClickItemSlot()
    {
        for (int i = 0; i < Slots.Length; ++i)
        {
            if (GetButtonName() == Slots[i].name)
            {
                // 인벤데이터의 해당 인덱스 정보를 넘겨줌
                GetGameObject((int)GameObjects.SelectItem_Panel).SetActive(true);
                selectItemUI.SetItemInfo(Slots[i].GetComponent<InvenSlot>().itemData);
            }
        }
    }

    public void SetSlotInfo(TestItemType _itemType)
    {
        int invenCount = Database.Instance.TestInvenList.Count;
        int lastIndex = 0;
        CurItemType = _itemType;
        ItemInven_CurItemType_txt.text = CurItemType.ToString();

        for (int i = 0; i < invenCount; ++i)
        {
            Slots[i].GetComponent<InvenSlot>().SetSlotInfo(null);

            for (int j = lastIndex; j < invenCount; ++j)
            {
                if (CurItemType == Database.Instance.TestInvenList[j].ItemType)
                {
                    Slots[i].GetComponent<InvenSlot>().SetSlotInfo(Database.Instance.TestInvenList[j]);
                    lastIndex = j + 1;
                    break;
                }

            }
        }
    }

    void SortItem()
    {

    }
}
