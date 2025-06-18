using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefenceUI : UIBase
{
    [SerializeField] private DefenceGameRule GameRule;
    [SerializeField] private DefenceCraftUI CraftUI;

    [SerializeField] private Button FieldActiveButton;
    [SerializeField] private Button FieldCraftPanelActiveButton;
    [SerializeField] private Button BaseActiveButton;
    [SerializeField] private Button BaseCraftPanelActiveButton;
    [SerializeField] private GameObject BottomBasePanel;
    [SerializeField] private GameObject BottomFieldPanel;

    [SerializeField] private TextMeshProUGUI Rount_txt;
    [SerializeField] private TextMeshProUGUI RoundTime_txt;

    [SerializeField] private Button UserSkillButton_0;
    [SerializeField] private Button UserSkillButton_1;


    private float RoundTime = 0f;

    private void Start()
    {
        FieldActiveButton.onClick.AddListener(OnClickFieldButton);
        BaseActiveButton.onClick.AddListener(OnClickBaseButton);
        FieldCraftPanelActiveButton.onClick.AddListener(() => { CraftUI.OnActivePanel(true); });
        BaseCraftPanelActiveButton.onClick.AddListener(() => { CraftUI.OnActivePanel(false); });
        GameRule.NextRoundAction += OnChangedRoundUI;
    }

    void Update()
    {
        RoundTime_txt.text = $"{(int)RoundTime}";
        RoundTime -= Time.deltaTime;
    }

    void OnChangedRoundUI()
    {
        Rount_txt.text = $"Round {GameRule.round}";
        RoundTime = GameRule.roundTime;
    }

    void OnClickBaseButton()
    {
        Camera.main.transform.position = new Vector3(-100, 200, -10); // ComandCenterPos
        BottomBasePanel.SetActive(true);
        BottomFieldPanel.SetActive(false);
    }

    void OnClickFieldButton()
    {
        Camera.main.transform.position = new Vector3(0, 0, -10); // FieldCenterPos
        BottomBasePanel.SetActive(false);
        BottomFieldPanel.SetActive(true);
    }

    //void OnChangedStoneCountUI()
    //{
    //    GameRule.HaveGreenStoneCount;
    //    GameRule.HaveBlueStoneCount;
    //    GameRule.HaveRedStoneCount;
    //}

    void OnClickSkillButton()
    {
        //if(GetButtonName == )
    }
}
