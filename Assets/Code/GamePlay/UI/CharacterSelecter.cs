using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelecter : UIBase
{
    [SerializeField]
    private GameObject ScrollView;

    private GameObject CurSlot;
    private GameObject[] Slots;

    public int SelectCharacterID { get; private set; }

    private void Awake()
    {
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

        GetButton((int)Buttons.Survival_Ready_Play_btn).onClick.AddListener(() => { GameManager.Instance.OpenScene("VampireSurvival", SelectCharacterID.ToString()); });
    }

    void Start()
    {
        ActiveSlot();
    }

    void Update()
    {
        
    }

    void ActiveSlot()
    {
        for(int i = 0; i < Database.Instance.CharacterAssetData.Count; ++i)
        {
            Slots[i].SetActive(true);
            Slots[i].GetComponent<CharacterSlot>().SetSlotInfo(i);
        }
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

    void OnClickSelectButton()
    {
        CharacterSlot slot = CurSlot.GetComponent<CharacterSlot>();
        SelectCharacterID = slot.CharacterID;

        PanelBackAction();
    }
}
