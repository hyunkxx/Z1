using UnityEngine;
using UnityEngine.UI;

public class ItemInventoryUI : UIBase
{
    [SerializeField] private GameObject MenuTab;
    [SerializeField] private GameObject Contents;
    private GameObject[] Slots;
    private Button[] TabButtons;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        int TabSize = MenuTab.transform.childCount; 
        int ContentsSize = Contents.transform.childCount;

        Slots = new GameObject[ContentsSize];
        TabButtons = new Button[TabSize];

        for (int i = 0; i < ContentsSize; ++i)
        {
            Slots[i] = Contents.transform.GetChild(i).gameObject;
            Contents.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(OnClickItemSlot);
        }

        for (int i = 0; i < TabSize; ++i)
        {
            TabButtons[i] = MenuTab.transform.GetChild(i).gameObject.GetComponent<Button>();
            MenuTab.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(OnClickTabButton);
        }

        GetButton((int)Buttons.ItemInven_Close_btn).onClick.AddListener(PanelBackAction);
        GetButton((int)Buttons.ItemInven_Sort_btn).onClick.AddListener(SortItem);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnClickTabButton()
    {
        for (int i = 0; i < Slots.Length; ++i)
        {
            if (GetButtonName() == Slots[i].name)
            {

            }
        }
    }


    void OnClickItemSlot()
    {
        for (int i = 0; i < Slots.Length; ++i)
        {
            if (GetButtonName() == Slots[i].name)
            {
                // 인벤데이터의 해당 인덱스 정보를 넘겨줌
                GetGameObject((int)GameObjects.SelectItem_Panel).SetActive(true);
            }
        }
    }

    void SortItem()
    {

    }

}
