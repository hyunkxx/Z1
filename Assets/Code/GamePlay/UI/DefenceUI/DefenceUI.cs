using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefenceUI : UIBase
{
    [SerializeField] private DefenceGameRule GameRule;
    [SerializeField] private Button CraftPanelActiveButton;
    [SerializeField] private GameObject CraftPanel;
    [SerializeField] private TextMeshProUGUI Rount_txt;
    [SerializeField] private TextMeshProUGUI RoundTime_txt;

    [SerializeField] private Button UserSkillButton_0;
    [SerializeField] private Button UserSkillButton_1;


    private float RoundTime = 0f;

    private void Start()
    {
        CraftPanelActiveButton.onClick.AddListener(() => { CraftPanel.SetActive(true); });
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
