using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DefenceUI : UIBase
{
    [SerializeField] private DefencePlayerController Controller;
    [SerializeField] private DefenceGameRule GameRule;
    [SerializeField] private DefenceCraftUI CraftUI;

    [Header("ResultPanel")]
    public GameObject WinPanel;
    [SerializeField] private Button WinRewardButton;
    public GameObject LosePanel;
    [SerializeField] private Button LoseRewardButton;

    [Header("Bottom Group")]
    [SerializeField] private Button FieldActiveButton;
    [SerializeField] private Button FieldCraftPanelActiveButton;
    [SerializeField] private Button BaseActiveButton;
    [SerializeField] private Button BaseCraftPanelActiveButton;
    [SerializeField] private GameObject BottomBasePanel;
    [SerializeField] private GameObject BottomFieldPanel;

    [Header("Text Group")]
    [SerializeField] private TextMeshProUGUI Rount_txt;
    [SerializeField] private TextMeshProUGUI RoundTime_txt;
    [SerializeField] private TextMeshProUGUI Gold_txt;

    [Header("User Interface Group")]
    [SerializeField] private Button UserSkillButton_0;
    [SerializeField] private Button UserSkillButton_1;


    private float RoundTime = 0f;

    private void Awake()
    {
        Controller = (DefencePlayerController)GameManager.Instance.GameMode.PlayerController;
    }

    private void Start()
    {
        FieldActiveButton.onClick.AddListener(OnClickFieldButton);
        FieldCraftPanelActiveButton.onClick.AddListener(() => { CraftUI.OnActivePanel(true); });

        BaseActiveButton.onClick.AddListener(OnClickBaseButton);
        BaseCraftPanelActiveButton.onClick.AddListener(() => { CraftUI.OnActivePanel(false); });

        WinRewardButton.onClick.AddListener(() => { GameRule.GetReward(DefenceGameRule.ERewardType.Win); });
        LoseRewardButton.onClick.AddListener(() => { GameRule.GetReward(DefenceGameRule.ERewardType.Lose); });

        UserSkillButton_0.onClick.AddListener(OnClickSkillButton);
        UserSkillButton_1.onClick.AddListener(OnClickSkillButton);

        GameRule.NextRoundAction += OnChangedRoundUI;
    }

    void Update()
    {
        RoundTime_txt.text = $"{(int)RoundTime}";
        RoundTime -= Time.deltaTime;

        OnChangedStoneCountUI();
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

    void OnChangedStoneCountUI()
    {
        Gold_txt.text = $"{DefenceGameRule.HaveGreenStoneCount}";
    }

    void OnClickSkillButton()
    {
        switch(GetButtonName())
        {
            case "SkillButton_0":
                Debug.Log("Skill_0 Active");
                Controller.Skill_0.
                break;

            case "SkillButton_1":
                Debug.Log("Skill_1 Active");
                break;
        }
    }
}
