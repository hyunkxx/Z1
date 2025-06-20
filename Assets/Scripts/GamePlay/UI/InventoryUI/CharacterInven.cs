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
        for (int i = 0; i < Database.Instance.CharacterAssetCount; ++i)
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
