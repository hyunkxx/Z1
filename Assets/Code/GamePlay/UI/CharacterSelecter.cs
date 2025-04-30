using UnityEngine;
using UnityEngine.UI;

public class CharacterSelecter : UIBase
{
    GameObject CurSlot; 
    private void Awake()
    {
        GetButton((int)Buttons.CharacterSelect_btn).onClick.AddListener(() => { PanelAction(GetGameObject((int)GameObjects.CharacterSelect_Panel)); });

        for(int i = 0; i < GetGameObject((int)GameObjects.CharacterSelectContents).transform.childCount; ++i)
        {
            GetGameObject((int)GameObjects.CharacterSelectContents).transform.GetChild(i).GetComponent<Button>().onClick.AddListener(OnClickCharacterSlot);
        }

        CurSlot = GetGameObject((int)GameObjects.CharacterSelectContents).transform.GetChild(0).gameObject;
        CurSlot.GetComponent<CharacterSlot>().ActiveFocus();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
