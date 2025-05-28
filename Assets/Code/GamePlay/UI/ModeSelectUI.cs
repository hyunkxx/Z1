using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ModeSelectUI : UIBase
{
    [SerializeField] private GameObject Contents;
    private GameObject[] Slots;

    string SelectMode = "Easy";

    void Start()
    {
        //Defence_Ready_Panel
        GetButton((int)Buttons.Defence_Ready_Play_btn).onClick.AddListener(() => { GameManager.Instance.OpenScene("Defence", SelectMode); });
        GetButton((int)Buttons.Defence_Ready_Back_btn).onClick.AddListener(PanelBackAction);
        GetButton((int)Buttons.Defence_ModeSelect_Back_btn).onClick.AddListener(PanelBackAction);

        int ContentsSize = Contents.transform.childCount;
        Slots = new GameObject[ContentsSize];
        for (int i = 0; i < ContentsSize; ++i)
        {
            Slots[i] = Contents.transform.GetChild(i).gameObject;
            Slots[i].GetComponent<Button>().onClick.AddListener(OnClickCharacterSlot);
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
                SelectMode = Slots[i].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text;
                Debug.Log(SelectMode);
                GetGameObject((int)GameObjects.Defence_Ready_Panel).SetActive(true);
            }
        }
    }

}
