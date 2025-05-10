using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelecter : UIBase
{
    GameObject CurSlot;
    [SerializeField] private GameObject ScrollView;
    

    private GameObject[] Slots;

    public void ActiveButton(bool value)
    {
        //GetButton((int)Buttons.CharacterSelect_Back_btn).gameObject.SetActive(value);
        GridUpdate();
    }

    
    private void Awake()
    {
        //GetButton((int)Buttons.CharacterSelect_Back_btn).onClick.AddListener(PanelBackAction);
        // GetButton((int)Buttons.CharacterSelect_btn).onClick.AddListener(PanelBackAction);
        GetButton((int)Buttons.CharacterSelect_btn).onClick.AddListener(OnClickSelectButton);

        int ContentsSize = GetGameObject((int)GameObjects.CharacterSelectContents).transform.childCount;
        Slots = new GameObject[ContentsSize];
        for (int i = 0; i < ContentsSize; ++i)
        {
            Slots[i] = GetGameObject((int)GameObjects.CharacterSelectContents).transform.GetChild(i).gameObject;
            GetGameObject((int)GameObjects.CharacterSelectContents).transform.GetChild(i).GetComponent<Button>().onClick.AddListener(OnClickCharacterSlot);
        }

        CurSlot = GetGameObject((int)GameObjects.CharacterSelectContents).transform.GetChild(0).gameObject;
        CurSlot.GetComponent<CharacterSlot>().ActiveFocus();
    }

    void Start()
    {
        //GridUpdate();
        ActiveSlot();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ActiveSlot()
    {
        for(int i = 0; i < Database.Instance.CharcterList.Count; ++i)
        {
            Slots[i].SetActive(true);
            SetSlotInfo(i);
        }
    }

    void SetSlotInfo(int _index)
    {
        Slots[_index].GetComponent<CharacterSlot>().CharacterID = Database.Instance.CharcterList[_index].ID;
        Slots[_index].GetComponent<CharacterSlot>().CharacterName = Database.Instance.CharcterList[_index].Name;
        Slots[_index].GetComponent<CharacterSlot>().CharacterImage.sprite = Database.Instance.CharcterList[_index].sprite;
    }

    void OnClickCharacterSlot()
    {
        GameObject SlotGroup = GetGameObject((int)GameObjects.CharacterSelectContents);
        for (int i = 0; i < SlotGroup.transform.childCount; ++i)
        {
            if (GetButtonName() == SlotGroup.transform.GetChild(i).name)
            {
                CurSlot.GetComponent<CharacterSlot>().ActiveFocus();
                CurSlot = SlotGroup.transform.GetChild(i).gameObject;
                SlotGroup.transform.GetChild(i).GetComponent<CharacterSlot>().ActiveFocus();
                GetImage((int)Images.Survival_Ready_CharcterSlot_Img).sprite = SlotGroup.transform.GetChild(i).GetComponent<CharacterSlot>().GetSprite();
            }
        }
    }

    private void GridUpdate()
    {
        float scrollWidth = ScrollView.GetComponent<RectTransform>().rect.width;
        float scrollHeight = ScrollView.GetComponent<RectTransform>().rect.height;

        float slotWidth = CurSlot.GetComponent<RectTransform>().rect.width;
        float slotHeight = CurSlot.GetComponent<RectTransform>().rect.height;


        int ContentsSize = GetGameObject((int)GameObjects.CharacterSelectContents).transform.childCount;
        int RowSize = (int)((scrollWidth * 0.8f) / slotWidth)+1;
        int Rowcnt = 1;

        float positionX = scrollWidth * 0.05f + slotWidth * 0.5f;
        float positionY = -slotHeight * 0.7f;


        Vector3 position = new Vector3(positionX,positionY,0);

        CurSlot.GetComponent<RectTransform>().localPosition = position;
        for (int i = 0; i < ContentsSize; ++i)
        {
            if (Slots[i] == CurSlot)
                continue;

            if(Rowcnt == RowSize)
            {
                Rowcnt = 0;
                positionY -= (slotHeight * 1.2f);
                position.y = positionY;
            }
            position.x = positionX + ((slotWidth + (scrollWidth *0.1f)/(RowSize))* Rowcnt);
            ++Rowcnt;
            Slots[i].GetComponent<RectTransform>().localPosition = position;
             
        }

        
        Vector2 vec2 = new Vector2(0, -position.y + slotHeight);
        GetGameObject((int)GameObjects.CharacterSelectContents).GetComponent<RectTransform>().sizeDelta = vec2;

    }


    void OnClickSelectButton()
    {
        GameManager.Instance.tempPlayerPrefab = (GameObject)AssetDatabase.LoadAssetAtPath($"Assets/Level/Prefabs/Character/Character_{CurSlot.GetComponent<CharacterSlot>().CharacterName}.prefab", typeof(GameObject));
        PanelBackAction();
    }
}
