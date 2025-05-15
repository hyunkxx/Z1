using UnityEngine;
using UnityEngine.UI;

public class CharacterInven : UIBase
{
    [SerializeField] private CharacterEquipUI characterEquipUI;
    [SerializeField] private GameObject CharacterInvenContents;

    private GameObject[] Slots;
    private GameObject CurSlot;
    //int CurSelectIndex = 0;


    private void Awake()
    {
       
    }

    private void Start()
    {
        int ContentsSize = CharacterInvenContents.transform.childCount;


        Slots = new GameObject[ContentsSize];
        for (int i = 0; i < ContentsSize; ++i)
        {
            Slots[i] = CharacterInvenContents.transform.GetChild(i).gameObject;
            CharacterInvenContents.transform.GetChild(i).GetComponent<Button>().onClick.AddListener(OnClickCharacterSlot);
            GetButton((int)Buttons.CharacterInven_Close_btn).onClick.AddListener(()=> { GetGameObject((int)GameObjects.CharacterInven_Panel).SetActive(false); });
        }

        CurSlot = Slots[0];

        ActiveSlot();
    }

    private void Update()
    {
        
    }

    void ActiveSlot()
    {
        for (int i = 0; i < Database.Instance.TestCharcterList.Count; ++i)
        {
            Slots[i].SetActive(true);
            Slots[i].GetComponent<CharacterSlot>().SetSlotInfo(i);
        }
    }

    void OnClickCharacterSlot()
    {
        for (int i = 0; i < Slots.Length; ++i)
        {
            if (GetButtonName() == Slots[i].name)
            {
                CurSlot.GetComponent<CharacterSlot>().ActiveFocus();
                CurSlot = Slots[i];
                Slots[i].GetComponent<CharacterSlot>().ActiveFocus();
                GetImage((int)Images.CharacterEquip_Character_Img).sprite = Slots[i].GetComponent<CharacterSlot>().GetSprite();

                SetSprite(Slots[i].GetComponent<CharacterSlot>().GetSprite());
                SetEquipInfo(Slots[i].GetComponent<CharacterSlot>().CharacterID);
            }
        }
    }

    public float targetVisualSize = 64f;

    public void SetSprite(Sprite newSprite)
    {
        Vector2 spriteSize = newSprite.rect.size;

        float maxAxis = Mathf.Max(spriteSize.x, spriteSize.y);
        float scale = targetVisualSize / maxAxis;

        Vector2 newSize = (spriteSize * scale) * 5.5f;
        GetImage((int)Images.CharacterEquip_Character_Img).rectTransform.sizeDelta = new Vector2(newSize.x, newSize.x);
    }

    void SetEquipInfo(int _id)
    {
        // Equip 스크립트에 현재 선택된 캐릭터의 정보를 전달한다.
        characterEquipUI.UpdateSlotUI(_id);
    }
}


//[SerializeField] private GameObject CharacterSelecter;
//[SerializeField] private Button[] OpenButtons;
//[SerializeField] private Button[] CloseButtons;

//private void Awake()
//{
//    for (int i = 0; i < OpenButtons.Length; i++)
//    {
//        OpenButtons[i].onClick.AddListener(Link_On);
//    }

//    for (int i = 0; i < CloseButtons.Length; i++)
//    {
//        CloseButtons[i].onClick.AddListener(Link_Off);
//    }

//    //GetButton((int)Buttons.CharacterInven_Back_btn).onClick.AddListener(PanelBackAction);
//}
//void Start()
//{
//    Link_On();
//}


//private void Link_On()
//{
//    Vector2 Minvec2 = new Vector2(0.5f, 0);
//    Vector2 Maxvec2 = new Vector2(1, 1);
//    Vector2 DeltaVec2 = new Vector2(0, 0);
//    PanelOpenAction(CharacterSelecter, OrderType.Multiple);

//    CharacterSelecter.GetComponent<RectTransform>().anchorMin = Minvec2;
//    CharacterSelecter.GetComponent<RectTransform>().anchorMax = Maxvec2;
//    CharacterSelecter.GetComponent<RectTransform>().sizeDelta = DeltaVec2;

//    CharacterSelecter.GetComponent<CharacterSelecter>().ActiveButton(false);
//    CharacterSelecter.transform.SetAsLastSibling();
//}

//private void Link_Off()
//{
//    Vector2 Minvec2 = new Vector2(0f, 0);
//    Vector2 Maxvec2 = new Vector2(1, 1);
//    Vector2 DeltaVec2 = new Vector2(0, 0);

//    CharacterSelecter.GetComponent<RectTransform>().anchorMin = Minvec2;
//    CharacterSelecter.GetComponent<RectTransform>().anchorMax = Maxvec2;
//    CharacterSelecter.GetComponent<RectTransform>().sizeDelta = DeltaVec2;

//    CharacterSelecter.GetComponent<CharacterSelecter>().ActiveButton(true);
//}
